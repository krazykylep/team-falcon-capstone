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

    /// <summary>
    /// These are global variables and default values used for DBOWrappers.
    /// </summary>
    public static class GlobVar
    {
        // identity is assigned when an Object is created or retrieved in DB
        public const int UNASSIGNED_IDENTITY = -1;
        public const ProcessState DEFAULT_PROCESS_STATE = ProcessState.GREEN;
        public const Priority DEFAULT_PRIORITY = Priority.MEDIUM;
        public const Category DEFAULT_CATEGORY = Category.INFORMATION;
        public const int DEFAULT_PLANNED_COUNT = 0;
        public const int DEFAULT_COMPLETED_COUNT = 0;
        public const double DEFAULT_PERCENT_COUNT = 0.0;
    }

    /// <summary>
    /// This is a wrapper class for Process objects.
    /// </summary>
    public class ProcessWrapper : ProcessDTO
    {
        private PeregrineDBDataContext db = new PeregrineDBDataContext();
        
        // This constructor will retrieve a process from the DB by ProcessName or add it
        // to the DB if it's not already there.
        public ProcessWrapper(string procName)
        {
            List<Process> result = db.GetProcessByName(procName).ToList<Process>();
            int resultCount = result.Count();

            if (resultCount == 0)       // add process to DB
            {
                ISingleResult<InsertProcessResult> insResult = db.InsertProcess(GlobVar.UNASSIGNED_IDENTITY, procName, (int)GlobVar.DEFAULT_PROCESS_STATE);
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
            ISingleResult<GetProcessResult> result = db.GetProcess(id);
            // should only have one proc in result
            foreach (GetProcessResult proc in result)
            {
                ProcessId = proc.ProcessID;
                ProcessName = proc.ProcessName;
                State = (ProcessState)proc.State;
            }
        }

        public void Update(ProcessState state)
        {
            State = state;

            ISingleResult<UpdateProcessResult> result = db.UpdateProcess(ProcessId, ProcessName, (int)State);
            // repopulate Properties?
        }

        public void DeleteFromDatabase()
        {
            db.DeleteProcess(ProcessId);
        }
    }

    /// <summary>
    /// This is a wrapper class for Job objects.
    /// </summary>
    public class JobWrapper : JobDTO
    {
        private PeregrineDBDataContext db = new PeregrineDBDataContext();

        public JobWrapper(string jobName)
        {
            List<Job> result = db.GetJobByName(jobName).ToList<Job>();
            int resultCount = result.Count();

            if (resultCount == 0)       // add process to DB
            {
                InsertJobResult insResult = db.InsertJob(GlobVar.UNASSIGNED_IDENTITY, jobName, GlobVar.DEFAULT_PLANNED_COUNT, GlobVar.DEFAULT_COMPLETED_COUNT, GlobVar.DEFAULT_PERCENT_COUNT).First();
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
            PlannedCount = GlobVar.DEFAULT_PLANNED_COUNT;

            ISingleResult<UpdateJobResult> result = db.UpdateJob(JobId, JobName, PlannedCount, (int)((double)PlannedCount*PercentComplete*0.01), PercentComplete);
            // repopulate Properties?
        }

        public void Update(int total, int completed)
        {
            if (total == 0) PercentComplete = 0.0;      // Should PercentComplete = 0 or 100, or throw exception?
            else PercentComplete = ((double)completed/(double)total) * 100.0;
            PlannedCount = total;

            ISingleResult<UpdateJobResult> result = db.UpdateJob(JobId, JobName, PlannedCount, completed, PercentComplete);
            // repopulate Properties?
        }

        public void DeleteFromDatabase()
        {
            db.DeleteJob(JobId);
        }
    }

    /// <summary>
    /// This is a wrapper class for Message objects.
    /// </summary>
    public class MessageWrapper : MessageDTO
    {
        private PeregrineDBDataContext db = new PeregrineDBDataContext();

        public MessageWrapper(String message, Category category, Priority priority)
        {
            InsertMessageResult result = db.InsertMessage(GlobVar.UNASSIGNED_IDENTITY, message, DateTime.Now, (int)category, (int)priority).First();

            MessageId = result.MessageID;
            Message = result.Message;
            Date = result.Date;
            Category = (Category)result.Category;
            Priority = (Priority)result.Priority;
        }
        
        public MessageWrapper(int id)
        {
            ISingleResult<Message> result = db.GetMessage(id);
            // should only have one message in result
            foreach (Message mess in result)
            {
                MessageId = mess.MessageID;
                Message = mess.Message1;
                Date = mess.Date;
                Category = (Category)mess.Category;
                Priority = (Priority)mess.Priority;
            }
        }

        public void DeleteFromDatabase()
        {
            db.DeleteMessage(MessageId);
        }
    }

    /// <summary>
    /// This is a wrapper class for For message/job/process relationships.
    /// </summary>
    public class LogRelWrapper
    {
        private int messageId;
        private int processId;
        private int jobId;

        private PeregrineDBDataContext db = new PeregrineDBDataContext();

        public LogRelWrapper(int messageId, int processId)
        {
            LogRel result = db.InsertLogRel(messageId, processId, null).First();

            MessageId = result.MessageID;
            ProcessId = result.ProcessID;
            JobId = GlobVar.UNASSIGNED_IDENTITY;
        }

        public LogRelWrapper(int messageId, int processId, int jobId)
        {
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


    /// <summary>
    /// Used to implement API calls for creating log messages.
    /// </summary>
    public class DBLogWrapper
    {
        public DBLogWrapper()
        {
        }

        public void logProcessMessage(String processName, String message, Category category, Priority priority)
        {
            message = processName + ": " + message;
            
            ProcessWrapper proc = new ProcessWrapper(processName);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId); 
        }

        public void logJobProgressAsPercentage(String jobName, String processName, double percent)
        {
            Category category = Category.PROGRESS;
            Priority priority = GlobVar.DEFAULT_PRIORITY;

            ProcessWrapper proc = new ProcessWrapper(processName);
            JobWrapper job = new JobWrapper(jobName);

            String message = processName + ", " + jobName
                + ": Job " + ((int)percent).ToString() + "% complete";

            job.Update(percent);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId, job.JobId); 
        }

        public void logJobProgress(String jobName, String processName, int total, int completed)
        {
            Category category = Category.PROGRESS;
            Priority priority = GlobVar.DEFAULT_PRIORITY;

            ProcessWrapper proc = new ProcessWrapper(processName);
            JobWrapper job = new JobWrapper(jobName);

            String message = processName + ", " + jobName
                + ": " + completed.ToString() + " of " + total.ToString() + " job tasks completed";

            job.Update(total, completed);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId, job.JobId); 
        }

        public void logJobStart(String jobName, String processName)
        {
            Category category = Category.PROGRESS;
            Priority priority = GlobVar.DEFAULT_PRIORITY;

            ProcessWrapper proc = new ProcessWrapper(processName);
            JobWrapper job = new JobWrapper(jobName);

            String message = processName + ", " + jobName
                + ": Job started";

            // Reset progress, just in case the job is already in the database.
            job.Update(GlobVar.DEFAULT_PLANNED_COUNT, GlobVar.DEFAULT_COMPLETED_COUNT);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId, job.JobId); 
        }

        public void logJobStartWithTotalTasks(String jobName, String processName, int totalTasks)
        {
            Category category = Category.PROGRESS;
            Priority priority = GlobVar.DEFAULT_PRIORITY;
            int completed = 0;

            ProcessWrapper proc = new ProcessWrapper(processName);
            JobWrapper job = new JobWrapper(jobName);

            String message = processName + ", " + jobName
                + ": Job started with " + totalTasks.ToString() + " total tasks";

            job.Update(totalTasks, completed);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId, job.JobId);
        }

        public void logJobComplete(String jobName, String processName)
        {
            Category category = Category.PROGRESS;
            Priority priority = GlobVar.DEFAULT_PRIORITY;
            double percent = 100.0;

            ProcessWrapper proc = new ProcessWrapper(processName);
            JobWrapper job = new JobWrapper(jobName);

            String message = processName + ", " + jobName + ": Job Completed";

            job.Update(percent);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId, job.JobId);
        }

        public void logProcessStart(String processName)
        {
            Category category = Category.START;
            Priority priority = GlobVar.DEFAULT_PRIORITY;
            ProcessState startState = GlobVar.DEFAULT_PROCESS_STATE;

            ProcessWrapper proc = new ProcessWrapper(processName);

            String message = processName + ": Process Started";

            // Reset state, just in case the process is already in the database.
            proc.Update(startState);

            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId);
        }

        public void logProcessShutdown(String processName)
        {
            Category category = Category.STOP;
            Priority priority = GlobVar.DEFAULT_PRIORITY;

            ProcessWrapper proc = new ProcessWrapper(processName);

            String message = processName + ": Process shut down";
            
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId);
        }

        public void logProcessStateChange(String processName, ProcessState state)
        {
            Category category = Category.STATE_CHANGE;
            Priority priority = GlobVar.DEFAULT_PRIORITY;

            ProcessWrapper proc = new ProcessWrapper(processName);

            String message = processName + ": Process state changed from " + ProcessStateToString(proc.State)
                + " to " + ProcessStateToString(state);
            
            proc.Update(state);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId);
        }

        public String ProcessStateToString(ProcessState state)
        {
            String returnString;

            switch (state)
            {
                case ProcessState.GREEN:
                    returnString = "GREEN";
                    break;
                case ProcessState.RED:
                    returnString = "RED";
                    break;
                case ProcessState.YELLOW:
                    returnString = "YELLOW";
                    break;
                default:
                    returnString = "Bad ProcessState!";
                    break;
            }

            return returnString;
        }

    }
}