using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RUnit.Introspection
{
    public class RMock<T> where T : class
    {
        // This will store the generated mock instance
        public T Instance { get; }
        
        // Private constructor as mock will be constructed using builder pattern
        private RMock(T internalInstance)
        {
            Instance = internalInstance;
        }
        public static RMockBuilder Builder()
        {
            return RMockBuilder.Define();
        }
        public class RMockBuilder
        {
            public static RMockBuilder Define()
            {
                return new RMockBuilder();
            }
            // Reference to the interface type we're going to mock
            Type type;
            /* 
             * TypeBuilder object which will store our mock type definition until we're ready to compile it.
             * This is the heart of our builder pattern with the RMockBuilder object only being used to keep this reference alive until we've
             * finished defining the mock object.
             */
            TypeBuilder builder;
            // We're going to store the responses which we define for all of our methods
            Dictionary<string, object> responses = new Dictionary<string, object>();
            // We also need to store references to the generated fields which will hold our return values
            Dictionary<string, FieldInfo> fields = new Dictionary<string, FieldInfo>();

            private RMockBuilder()
            {
                this.type = typeof(T);
                StartBuild();
            }
            private void StartBuild()
            {
                // Get the assembly defining the type supplied
                AssemblyName assemblyName = type.Assembly.GetName();
                // Create an AssemblyBuilder object
                AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                // Get a ModuleBuilder - assemblies may be constructed of multiple modules but this is annoying and not used
                ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
                // Define a new type which will impliment out supplied interface type - it is sealed so it cannot be subclassed
                builder = moduleBuilder.DefineType($"Mock{type.Name}",
                    TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed);
                // Attach the interface to the new type so it knows it must impliment it
                builder.AddInterfaceImplementation(type);
            }
            public RMockBuilder DefineReturn<TResult>(string methodName, TResult returnValue)
            {
                // Check we've not already tried to define a response from this method
                if (!responses.ContainsKey(methodName))
                {
                    responses.Add(methodName, returnValue);

                    // Get method signature from type
                    var method = type.GetMethod(methodName);

                    // Define method - same parameters, same return
                    MethodBuilder methodBuilder = builder.DefineMethod($"{method.Name}",
                            MethodAttributes.Public | MethodAttributes.Virtual,
                            method.ReturnType,
                            method.GetParameters().Select(p => p.ParameterType).ToArray());

                    // Get IL Generator object
                    ILGenerator il = methodBuilder.GetILGenerator();

                    // Load "this"
                    il.Emit(OpCodes.Ldarg_0);

                    // If method returns object line up response on stack
                    if (method.ReturnType != typeof(void))
                    {
                        // Create private field to store return value in
                        var field = builder.DefineField($"_{methodName}Result", method.ReturnType, FieldAttributes.Private | FieldAttributes.InitOnly);
                        fields.Add(method.Name, field);
                        il.Emit(OpCodes.Ldfld, field);
                    }
                    // Return
                    il.Emit(OpCodes.Ret);

                    // Set method definition on type now it's built
                    builder.DefineMethodOverride(methodBuilder, method);
                }
                return this;
            }
            public RMock<T> Compile()
            {
                // Generate a constructor to initialise all private fields
                BuildCtor();

                // Build the final type
                var builtType = builder.CreateType();

                // Create an instance of the newly defined type, pass in the originally supplied defined method results as parameters
                var instance = Activator.CreateInstance(builtType, responses.Values.ToArray()) as T;

                // Create and return the RMock wrapper object
                return new RMock<T>(instance);
            }
            private void BuildCtor()
            {
                // Create a constructor builder object
                ConstructorBuilder constructor = builder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, responses.Values.Select(v => v.GetType()).ToArray());
                ILGenerator il = constructor.GetILGenerator();

                // Load "this"
                il.Emit(OpCodes.Ldarg_0);
                // Call base type constructor - System.Object - consumes slot of "this"
                il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
                
                // Loop through all values which we know will be supplied at construction time
                for (int i = 0; i < responses.Count; i++)
                {
                    // Load "this" back into the stack
                    il.Emit(OpCodes.Ldarg_0);
                    // Load the parameter argument by inex - 0 is "this"
                    il.Emit(OpCodes.Ldarg, i + 1);                    
                    /* 
                     * Find the FieldInfo object we stored when we defined a particular method return 
                     * set it's value using the argument loaded into the current slot
                    */
                    il.Emit(OpCodes.Stfld, fields.ElementAt(i).Value);
                }
                // Return
                il.Emit(OpCodes.Ret);
            }
        }
    }
}
