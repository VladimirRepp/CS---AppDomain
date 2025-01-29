using System;
using System.Reflection;

// Обычно, операционная система и среда исполнения
// предоставляют некоторую форму изоляции приложений
// друг от друга. Это разделение необходимо для того,
// чтобы существовала некоторая степень уверенности,
// что код, исполняющийся в рамках одного приложения,
// не может повлиять на код, исполняющийся в другом, не
// связанном с ним, приложении. 
// Для изоляции исполняемого кода приложений операционная
// система Windows использует концепцию процессов. 

// Домены приложений используются для изоляции в
// области безопасности, надёжности, контроля версий, а
// так же для закрытия загруженных сборок в целях
// освобождения используемой ими памяти

// Возможность запускать несколько приложений в
// рамках одного процесса увеличивает масштабируемость и гибкость решения

// Прежде, чем выполнить некоторый код, необходимо загрузить сборку,
// которая его содержит в некоторый домен приложения. Как правило,
// приложение загружает сразу несколько сборок.


// Ниже будет приведён пример приложения, которое
// динамически создаёт домен приложения, загружает в
// него dll-библиотеку и выполняет метод из этой библиотеки.
// После того, как искомый метод будет выполнен и
// библиотека будет более не нужна, приложение отгружает,
// sсозданный им, домен приложения, а соответственно
// и все ресурсы, с ним связанные.
namespace Sample_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppDomain myDomain = AppDomain.CreateDomain("Demo Domain");
            Assembly asm = myDomain.Load(AssemblyName.GetAssemblyName("SampleLibrary.dll"));
            Module module = asm.GetModule("SampleLibrary.dll");
            Type SampleClass = module.GetType("SampleLibrary.SampleClass");
          
            // Работаем с библиотекой 
            // 3 разных варианта 
            MethodInfo method_DoSome = SampleClass.GetMethod("DoSome");
            method_DoSome.Invoke(null, null);

            MethodInfo method_DoSome1 = asm.GetModule("SampleLibrary.dll").
            GetType("SampleLibrary.SampleClass").
            GetMethod("DoSome");
            method_DoSome1.Invoke(null, null);

            asm.GetModule("SampleLibrary.dll").
            GetType("SampleLibrary.SampleClass").
            GetMethod("DoSome").
            Invoke(null, null);

            AppDomain.Unload(myDomain);

            Console.ReadLine();
        }
    }
}
