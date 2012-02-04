﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeregrineDBWrapper;
using PeregrineAPI;

namespace PeregrinDBTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            String testProcessName = "Test Process Name 1";
            String testProcessMessage = "Test Process Message 1";
            Category testProcessCategory = Category.INFORMATION;
            Priority testProcessPriority = Priority.MEDIUM;
            String testJobName1 = "Test Job Name 1";
            double testPercent = 50;
            String testJobName2 = "Test Job Name 2";
            int testTotal = 1000;
            int testCompleted = 100;


            DBLogWrapper log = new DBLogWrapper();

            log.logProcessMessage(testProcessName, testProcessMessage, testProcessCategory, testProcessPriority);

            log.logJobProgressAsPercentage(testJobName1, testProcessName, testPercent);

            log.logJobProgress(testJobName2, testProcessName, testTotal, testCompleted);

            // int id;

            // NEED TO MAKE CHANGES

            /*
            ProcessWrapper proc = new ProcessWrapper("TestClientProcess", (ProcessState)1);
            proc.PutInDatabase();
            Console.WriteLine("Put In Database: {0},{1},{2}", proc.ProcessId, proc.ProcessName, proc.State);
            id = proc.ProcessId;
            proc = new ProcessWrapper(id);
            Console.WriteLine("Pulled From Database: {0},{1},{2}", proc.ProcessId, proc.ProcessName, proc.State);
            proc.DeleteFromDatabase();
            Console.WriteLine("Deleted From Database: {0},{1},{2}", proc.ProcessId, proc.ProcessName, proc.State);

            JobWrapper job = new JobWrapper("TestClientJob", 1, 10, 0.1);
            job.PutInDatabase();
            Console.WriteLine("Put In Database: {0},{1},{2},{3}", job.JobId, job.JobName, job.PlannedCount, job.PercentComplete);
            id = job.JobId;
            job = new JobWrapper(id);
            Console.WriteLine("Pulled From Database: {0},{1},{2},{3}", job.JobId, job.JobName, job.PlannedCount, job.PercentComplete);
            job.DeleteFromDatabase();
            Console.WriteLine("Deleted From Database: {0},{1},{2},{3}", job.JobId, job.JobName, job.PlannedCount, job.PercentComplete);

            MessageWrapper mess = new MessageWrapper("TestClientMessage", DateTime.Now, (Category)1, (Priority)1);
            mess.PutInDatabase();
            Console.WriteLine("Put In Database: {0},{1},{2},{3},{4}", mess.MessageId, mess.Message, mess.Date, mess.Category, mess.Priority);
            id = mess.MessageId;
            mess = new MessageWrapper(id);
            Console.WriteLine("Pulled From Database: {0},{1},{2},{3},{4}", mess.MessageId, mess.Message, mess.Date, mess.Category, mess.Priority);
            mess.DeleteFromDatabase();
            Console.WriteLine("Deleted From Database: {0},{1},{2},{3},{4}", mess.MessageId, mess.Message, mess.Date, mess.Category, mess.Priority);
            */
            Console.ReadLine();
        }
    }
}
