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
    using System.Collections.Generic;

    public class ProcessWrapper : ProcessDTO
    {
        // identity is assigned when Process is created or retrieved from DB
        private const int           UNASSIGNED_IDENTITY = -1;     
        private const ProcessState  DEFAULT_STATE = ProcessState.GREEN;

        [Obsolete]
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
            List<Process> result = db.GetProcessByName(procName).ToList<Process>();
            int resultCount = result.Count();

            if (resultCount == 0)       // add process to DB
            {
                ISingleResult<InsertProcessResult> insResult = db.InsertProcess(UNASSIGNED_IDENTITY, procName, (int)DEFAULT_STATE);
                InsertProcessResult proc = insResult.First();
                ProcessId = proc.ProcessID;
                ProcessName = proc.ProcessName;
                State = (ProcessState)proc.State;
            }
            else if (resultCount == 1)  // retrieve process from DB
            {
                Process proc = result.First();
                ProcessId = proc.ProcessID;
                ProcessName = proc.ProcessName;
                State = (ProcessState)proc.State;
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

        [Obsolete]
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
        // identity is assigned when Job is created in DB
        private const int UNASSIGNED_IDENTITY = -1;
        private const int DEFAULT_PLANNED_COUNT = 100;
        private const int DEFAULT_COMPLETED_COUNT = 0;
        private const double DEFAULT_PERCENT_COUNT = 0.0;

        [Obsolete]
        public JobWrapper()
        {
            JobId = UNASSIGNED_IDENTITY;
            JobName = "";
            PlannedCount = 0;
            PercentComplete = 0;
            PercentComplete = 0;
        }

        public JobWrapper(string jobName)
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            List<Job> result = db.GetJobByName(jobName).ToList<Job>();
            int resultCount = result.Count();

            if (resultCount == 0)       // add process to DB
            {
                InsertJobResult insResult = db.InsertJob(UNASSIGNED_IDENTITY, jobName, DEFAULT_PLANNED_COUNT, DEFAULT_COMPLETED_COUNT, DEFAULT_PERCENT_COUNT).First();
                JobId = insResult.JobID;
                JobName = insResult.JobName;
                PlannedCount = (int)insResult.PlannedCount; // PlannedCount currently Nullable in DB
                // CompletedCount not in DTO
                PercentComplete = (double)insResult.PercentComplete;    // Nullable in DB?
            }
            else if (resultCount == 1)  // retrieve process from DB
            {
                Job job = result.First();
                JobId = job.JobID;
                JobName = job.JobName;
                PlannedCount = (int)job.PlannedCount; // PlannedCount currently Nullable in DB
                // CompletedCount not in DTO
                PercentComplete = (double)job.PercentComplete;    // Nullable in DB?
            }
            else                        // BAD: multiple ID's for given process name
            {
                // throw exception or something
            }
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

        public void Update(double percent)
        {
            PercentComplete = percent;

            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<UpdateJobResult> result = db.UpdateJob(JobId, JobName, PlannedCount, (int)((double)PlannedCount*PercentComplete*0.01), PercentComplete);
            // repopulate Properties?
        }

        public void Update(int total, int completed)
        {
            PercentComplete = ((double)completed/(double)total) * 100.0;
            PlannedCount = total;

            PeregrineDBDataContext db = new PeregrineDBDataContext();
            ISingleResult<UpdateJobResult> result = db.UpdateJob(JobId, JobName, PlannedCount, completed, PercentComplete);
            // repopulate Properties?
        }

        [Obsolete]
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

        [Obsolete]
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
            InsertMessageResult result = db.InsertMessage(UNASSIGNED_IDENTITY, message, DateTime.Now, (int)category, (int)priority).First();

            MessageId = result.MessageID;
            Message = result.Message;
            Date = result.Date;
            Category = (Category)result.Category;
            Priority = (Priority)result.Priority;
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

        [Obsolete]
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

    public class LogRelWrapper
    {
        private const int UNASSIGNED_IDENTITY = -1;     

        private int messageId;
        private int processId;
        private int jobId;

        public LogRelWrapper(int messageId, int processId)
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            LogRel result = db.InsertLogRel(messageId, processId, null).First();

            MessageId = result.MessageID;
            ProcessId = result.ProcessID;
            JobId = UNASSIGNED_IDENTITY;
        }

        public LogRelWrapper(int messageId, int processId, int jobId)
        {
            PeregrineDBDataContext db = new PeregrineDBDataContext();
            LogRel result = db.InsertLogRel(messageId, processId, jobId).First();

            MessageId = result.MessageID;
            ProcessId = result.ProcessID;
            JobId = (int)result.JobID;      //JobID is nullable in DB
        }

        public int MessageId
        {
            get
            {
                return messageId;
            }
            set
            {
                messageId = value;
            }
        }

        public int ProcessId
        {
            get
            {
                return processId;
            }
            set
            {
                processId = value;
            }
        }

        public int JobId
        {
            get
            {
                return jobId;
            }
            set
            {
                jobId = value;
            }
        }
    }

    // for DB retrieval
    public class DBSearchWrapper
    {
        public DBSearchWrapper()
        {
        }

        //MessageDTO getMessage(int msg_id);

        //List<ProcessDTO> getAllProcesses();

        //List<ProcessSummary> getSummaryByPage(int pageNumber, int num_to_fetch, SortBy sortBy);
    }

    // for DB insertion / alteration
    public class DBLogWrapper
    {
        Priority DEFAULT_PRIORITY = Priority.MEDIUM;

        public DBLogWrapper()
        {
        }

        public void logProcessMessage(String processName, String message, Category category, Priority priority)
        {
            ProcessWrapper proc = new ProcessWrapper(processName);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId); 
        }

        public void logJobProgressAsPercentage(String jobName, String processName, double percent)
        {
            String message = "generated JobProgressAsPercentage message";
            Category category = Category.PROGRESS;
            Priority priority = DEFAULT_PRIORITY;

            ProcessWrapper proc = new ProcessWrapper(processName);
            JobWrapper job = new JobWrapper(jobName);
            job.Update(percent);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId, job.JobId); 
        }

        public void logJobProgress(String jobName, String processName, int total, int completed)
        {
            String message = "generated JobProgress message";
            Category category = Category.PROGRESS;
            Priority priority = DEFAULT_PRIORITY;

            ProcessWrapper proc = new ProcessWrapper(processName);
            JobWrapper job = new JobWrapper(jobName);
            job.Update(total, completed);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId, job.JobId); 
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