using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ThreadPoolSample();
            // TaskSample();
            // TaskSampleT();
            AsyncAwaitSample();

        }

        /// <summary>
        /// 线程池举例
        /// </summary>
        static void ThreadPoolSample()
        {
            int i = 0;
            while (i < 300)
            {
                ThreadPool.QueueUserWorkItem(a =>
                {
                    Console.WriteLine("当前线程为" + Thread.CurrentThread.ManagedThreadId);
                    i++;
                });
            }
        }

        /// <summary>
        /// task举例
        /// </summary>
        static void TaskSample()
        {
            Console.WriteLine("主线程启动");
            //Task.Run启动一个线程
            //Task启动的是后台线程，要在主线程中等待后台线程执行完毕，可以调用Wait方法
            //Task task = Task.Factory.StartNew(() => { Thread.Sleep(1500); Console.WriteLine("task启动"); });
            Task task = Task.Run(() => {
                Thread.Sleep(1500);
                Console.WriteLine("task启动");
            });
            Thread.Sleep(300);
            // 主线程等待task执行完成
            task.Wait();
            Console.WriteLine("主线程结束");
        }

        /// <summary>
        /// task异步返回举例
        /// </summary>
        static void TaskSampleT()
        {
            Console.WriteLine("主线程开始");
            //返回值类型为string
            Task<string> task = Task<string>.Run(() => {
                Thread.Sleep(2000);
                return Thread.CurrentThread.ManagedThreadId.ToString();
            });
            //会等到task执行完毕才会输出;
            Console.WriteLine("task执行结果为:"+task.Result);
            Console.WriteLine("主线程结束");
        }

        /// <summary>
        /// 异步使用
        /// </summary>
        static void AsyncAwaitSample()
        {
            Console.WriteLine("-------主线程启动-------");
            // 此处表示开始调用异步方法，紧接着就继续执行主线程
            Task<int> task = GetStrLengthAsync();
            // 继续执行
            Console.WriteLine("主线程继续执行");
            // 此处代码表示必须等待task执行完成并且返回值
            Console.WriteLine("Task返回的值" + task.Result);
            Console.WriteLine("-------主线程结束-------");
        }


        static async Task<int> GetStrLengthAsync()
        {
            Console.WriteLine("GetStrLengthAsync方法开始执行");
            //此处返回的<string>中的字符串类型，而不是Task<string>
            string str = await GetString();
            Console.WriteLine("GetStrLengthAsync方法执行结束");
            return str.Length;
        }

        static Task<string> GetString()
        {
            //Console.WriteLine("GetString方法开始执行")
            return Task<string>.Run(() =>
            {
                Thread.Sleep(2000);
                return "GetString的返回值";
            });
        }

        static void GetAwaiterSample()
        {
            Console.WriteLine("主线程开始");
            Task<string> task = Task<string>.Run(() => {
                Thread.Sleep(2000);
                return Thread.CurrentThread.ManagedThreadId.ToString();
            });
            //会等到task任务执行完之后执行
            task.GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine(task.Result);
            });
            Console.WriteLine("主线程结束");
            Console.Read();
        }

        /// <summary>
        /// 举例task continue的使用
        /// </summary>
        static void ContinueWithSample()
        {
            Console.WriteLine("主线程开始");
            Task<string> task = Task<string>.Run(() => {
                Thread.Sleep(2000);
                return Thread.CurrentThread.ManagedThreadId.ToString();
            });

            task.GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine(task.Result);
            });
            task.ContinueWith(m => { Console.WriteLine("第一个任务结束啦！我是第二个任务"); });
            Console.WriteLine("主线程结束");
            Console.Read();
        }
    }
}
