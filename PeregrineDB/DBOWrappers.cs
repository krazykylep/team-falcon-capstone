﻿//------------------------------------------------------------------------------
// c. 2012 by Nicholas Benson
// This file contains wrappers for the Autogenerated DB objects
//-----------------------------------------------------------------------------

namespace PeregrineDBWrapper
{
    using System;
    using System.Data.Linq;
    using System.Linq;
    using PeregrineAPI;
    using PeregrineDB;

    public class ProcessWrapper : ProcessDTO
    {
        // identity is assigned when Process is created or retrieved from DB
        private const int           UNASSIGNED_IDENTITY = -1;     
        private const ProcessState  DEFAULT_STATE = ProcessState.GREEN;

        public ProcessWrapper()
        {
            ProcessId = UNASSIGNED_IDENTITY;
            ProcessName = "";
            State = DEFAULT_STATE;
        }

        // This constructor will retrieve a process from the DB by ProcessName or add it
        // to the DB if it's not already there.
        public ProcessWrapper(string procName)
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<Process> result = db.GetProcessByName(procName);
            int resultCount = result.Count();

            if (resultCount == 0)       // add process to DB
            {
                ISingleResult<InsertProcessResult> insResult = db.InsertProcess(UNASSIGNED_IDENTITY, procName, (int)DEFAULT_STATE);
                ProcessId = insResult.First().ProcessID;
                ProcessName = insResult.First().ProcessName;
                State = (ProcessState)insResult.First().State;
            }
            else if (resultCount == 1)  // retrieve process from DB
            {
                ProcessId = result.First().ProcessID;
                ProcessName = result.First().ProcessName;
                State = (ProcessState)result.First().State;
            }
            else                        // BAD: multiple ID's for given process name
            {
                // throw exception or something
            }
        }

        public ProcessWrapper(int id)
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<GetProcessResult> result = db.GetProcess(id);
            // should only have one proc in result
            foreach (GetProcessResult proc in result)
            {
                ProcessId = proc.ProcessID;
                ProcessName = proc.ProcessName;
                State = (ProcessState)proc.State;
            }
        }

        public void PutInDatabase()
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<InsertProcessResult> result = db.InsertProcess(null, ProcessName, (int)State);
            // should only have one proc in result
            foreach (InsertProcessResult proc in result)
            {
                ProcessId = proc.ProcessID;
                ProcessName = proc.ProcessName;
                State = (ProcessState)proc.State;
            }
        }

        public void DeleteFromDatabase()
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            db.DeleteProcess(ProcessId);
        }
    }

    public class JobWrapper : JobDTO
    {
        private const int UNASSIGNED_IDENTITY = -1;     // identity is assigned when
                                                        // Job is created in DB

        public JobWrapper()
        {
            JobId = UNASSIGNED_IDENTITY;
            JobName = "";
            PlannedCount = 0;
            PercentComplete = 0;
            PercentComplete = 0;
        }

        public JobWrapper(string jName, int pCount, int cCount, double pComplete)
        {
            JobId = UNASSIGNED_IDENTITY;
            JobName = jName;
            PlannedCount = pCount;
            PercentComplete = cCount;
            PercentComplete = pComplete;
        }

        public JobWrapper(int id)
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<GetJobResult> result = db.GetJob(id);
            // should only have one job in result
            foreach (GetJobResult job in result)
            {
                JobId = job.JobID;  
                JobName = job.JobName;
                PlannedCount = (int)job.PlannedCount;
                PercentComplete = (double)job.PercentComplete;
            }
        }

        public void PutInDatabase()
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<InsertJobResult> result = db.InsertJob(null, JobName, PlannedCount, 0, PercentComplete);
            // should only have one job in result
            foreach (InsertJobResult job in result)
            {
                JobId = job.JobID;
                JobName = job.JobName;
                PlannedCount = (int)job.PlannedCount;
                PercentComplete = (double)job.PercentComplete;
            }
        }

        public void DeleteFromDatabase()
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            db.DeleteJob(JobId);
        }
    }

    public class MessageWrapper : MessageDTO
    {
        // identity is assigned when Message is created in DB
        private const int UNASSIGNED_IDENTITY = -1;     
        private const Category DEFAULT_CATEGORY = Category.INFORMATION;
        private const Priority DEFAULT_PRIORITY = Priority.MEDIUM;

        public MessageWrapper()
        {
            MessageId = UNASSIGNED_IDENTITY;
            Message = "";      
            Date = DateTime.Now;
            Category = 0;
            Priority = 0;
        }

        public MessageWrapper(String message, Category category, Priority priority)
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<InsertMessageResult> result = db.InsertMessage(UNASSIGNED_IDENTITY, message, DateTime.Now, (int)category, (int)priority);

            MessageId = result.First().MessageID;
            Message = result.First().Message;
            Date = result.First().Date;
            Category = (Category)result.First().Category;
            Priority = (Priority)result.First().Priority;
        }
    
        public MessageWrapper(int id)
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<GetMessageResult> result = db.GetMessage(id);
            // should only have one message in result
            foreach (GetMessageResult mess in result)
            {
                MessageId = mess.MessageID;
                Message = mess.Message;
                Date = mess.Date;
                Category = (Category)mess.Category;
                Priority = (Priority)mess.Priority;
            }
        }

        public void PutInDatabase()
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<InsertMessageResult> result = db.InsertMessage(null, Message, Date, (int)Category, (int)Priority);
            // should only have one message in result
            foreach (InsertMessageResult mess in result)
            {
                MessageId = mess.MessageID;
                Message = mess.Message;
                Date = mess.Date;
                Category = (Category)mess.Category;
                Priority = (Priority)mess.Priority;
            }
        }

        public void DeleteFromDatabase()
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            db.DeleteMessage(MessageId);
        }
    }

    // for DB retrieval
    class DBSearchWrapper
    {
        public DBSearchWrapper()
        {
        }

        //MessageDTO getMessage(int msg_id);

        //List<ProcessDTO> getAllProcesses();

        //List<ProcessSummary> getSummaryByPage(int pageNumber, int num_to_fetch, SortBy sortBy);
    }

    // for DB insertion / alteration
    class DBLogWrapper
    {
        public DBLogWrapper()
        {
        }

        public void logProcessMessage(String processName, String message, Category category, Priority priority)
        {
            ProcessWrapper proc = new ProcessWrapper(processName);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            //LogRelWrapper rel = new LogRelWrapper(mess.MessageID, proc.ProcessID); 
        }

        public void logJobProgressAsPercentage(String jobName, String processName, int percent)
        {
        }

        public void logJobProgress(String jobName, String processName, int total, int completed)
        {
        }

        public void logJobStart(String jobName, String processName)
        {
        }

        public void logJobStartWithTotalTasks(String jobName, String processName, int totalTasks)
        {
        }

        public void logJobComplete(String jobName, String processName)
        {
        }

        public void logProcessStart(String processName)
        {
        }

        public void logProcessShutdown(String processName)
        {
        }

        public void logProcessStateChange(String processName, ProcessState state)
        {
        }

    }
}