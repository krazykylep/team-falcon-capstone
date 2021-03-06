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
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessWrapper"/> class.
        /// If given process name is in database, populated this object with database values.
        /// If given process name is not in database, populate this object with default
        /// values and store in database.
        /// </summary>
        /// <param name="procName">Name of a process.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessWrapper"/> class.
        /// Populate object with values stored in database matching given process id.
        /// </summary>
        /// <param name="id">The id for a process.</param>
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

        /// <summary>
        /// Updates the process state both for current object and in the database.
        /// </summary>
        /// <param name="state">The process state.</param>
        public void Update(ProcessState state)
        {
            State = state;

            ISingleResult<UpdateProcessResult> result = db.UpdateProcess(ProcessId, ProcessName, (int)State);
            // repopulate Properties?
        }

        [Obsolete]
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

        /// <summary>
        /// Initializes a new instance of the <see cref="JobWrapper"/> class.
        /// If given job name is in database, populated this object with database values.
        /// If given job name is not in database, populate this object with default
        /// values and store in database.
        /// </summary>
        /// <param name="jobName">Name of the Job.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="JobWrapper"/> class.
        /// Populate the object with values in database corresponding
        /// to the given Job ID.
        /// </summary>
        /// <param name="id">The Job's ID.</param>
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

        /// <summary>
        /// Updates the current job's progress to specified percent.
        /// Sets PlannedCount to default value and calculates
        /// the completed count based on percentage.
        /// Store current job progress in the database.
        /// </summary>
        /// <param name="percent">The percentage of Job completion.</param>
        public void Update(double percent)
        {
            PercentComplete = percent;
            PlannedCount = GlobVar.DEFAULT_PLANNED_COUNT;

            ISingleResult<UpdateJobResult> result = db.UpdateJob(JobId, JobName, PlannedCount, (int)((double)PlannedCount*PercentComplete*0.01), PercentComplete);
            // repopulate Properties?
        }

        /// <summary>
        /// Updates the jobs progress to specified total and completed counts.
        /// Calculates percentage complete and updates values in the database.
        /// </summary>
        /// <param name="total">The total number of Job tasks.</param>
        /// <param name="completed">The number of completed Job tasks.</param>
        public void Update(int total, int completed)
        {
            if (total == 0) PercentComplete = 0.0;      // Should PercentComplete = 0 or 100, or throw exception?
            else PercentComplete = ((double)completed/(double)total) * 100.0;
            PlannedCount = total;

            ISingleResult<UpdateJobResult> result = db.UpdateJob(JobId, JobName, PlannedCount, completed, PercentComplete);
            // repopulate Properties?
        }

        [Obsolete]
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageWrapper"/> class.
        /// Adds the message with given message, category, and priority
        /// to the database.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="category">The message category.</param>
        /// <param name="priority">The message priority.</param>
        public MessageWrapper(String message, Category category, Priority priority)
        {
            InsertMessageResult result = db.InsertMessage(GlobVar.UNASSIGNED_IDENTITY, message, DateTime.Now, (int)category, (int)priority).First();

            MessageId = result.MessageID;
            Message = result.Message;
            Date = result.Date;
            Category = (Category)result.Category;
            Priority = (Priority)result.Priority;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageWrapper"/> class.
        /// Populates the object with values from the database matching the given
        /// Message ID.
        /// </summary>
        /// <param name="id">The Message ID.</param>
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

        [Obsolete]
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

        /// <summary>
        /// Initializes a new instance of the <see cref="LogRelWrapper"/> class.
        /// Create a relationship between the given Message ID and Process ID
        /// and store that relationship in the database.
        /// </summary>
        /// <param name="messageId">The Message ID.</param>
        /// <param name="processId">The Process ID.</param>
        public LogRelWrapper(int messageId, int processId)
        {
            LogRel result = db.InsertLogRel(messageId, processId, null).First();

            MessageId = result.MessageID;
            ProcessId = result.ProcessID;
            JobId = GlobVar.UNASSIGNED_IDENTITY;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogRelWrapper"/> class.
        /// Create a relationship between the given Message ID, Process ID,
        /// and Job ID, and store that relationship in the database.
        /// </summary>
        /// <param name="messageId">The Message ID.</param>
        /// <param name="processId">The Process ID.</param>
        /// <param name="jobId">The Job ID.</param>
        public LogRelWrapper(int messageId, int processId, int jobId)
        {
            LogRel result = db.InsertLogRel(messageId, processId, jobId).First();

            MessageId = result.MessageID;
            ProcessId = result.ProcessID;
            JobId = (int)result.JobID;      //JobID is nullable in DB
        }

        /// <summary>
        /// Gets or sets the Message ID.
        /// </summary>
        /// <value>
        /// The Message ID.
        /// </value>
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

        /// <summary>
        /// Gets or sets the Process ID.
        /// </summary>
        /// <value>
        /// The Process ID.
        /// </value>
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

        /// <summary>
        /// Gets or sets the Job ID.
        /// </summary>
        /// <value>
        /// The Job ID.
        /// </value>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="DBLogWrapper"/> class.
        /// </summary>
        public DBLogWrapper()
        {
        }

        /// <summary>
        /// Logs a message for a given process.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        /// <param name="message">The message text.</param>
        /// <param name="category">The message category.</param>
        /// <param name="priority">The message priority.</param>
        public void logProcessMessage(String processName, String message, Category category, Priority priority)
        {
            message = processName + ": " + message;
            
            ProcessWrapper proc = new ProcessWrapper(processName);
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId); 
        }

        /// <summary>
        /// Logs the progress of a job, given as a percentage.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="processName">Name of the process associated with the job.</param>
        /// <param name="percent">The job progress as a percentage.</param>
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

        /// <summary>
        /// Logs the progress of a job, given as a number of completed tasks
        /// out of a total number of tasks.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="processName">Name of the process associated with the job.</param>
        /// <param name="total">The total number of tasks.</param>
        /// <param name="completed">The completed number of tasks.</param>
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

        /// <summary>
        /// Logs the starting of a job.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="processName">Name of the process associated with the job.</param>
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

        /// <summary>
        /// Logs the start of a job with a given number of total tasks.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="processName">Name of the process associated with the job.</param>
        /// <param name="totalTasks">The number of total tasks for this job.</param>
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

        /// <summary>
        /// Logs the completion of a job.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="processName">Name of the process associated with the job.</param>
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

        /// <summary>
        /// Logs the starting of a process.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
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

        /// <summary>
        /// Logs the shutdown of a process.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        public void logProcessShutdown(String processName)
        {
            Category category = Category.STOP;
            Priority priority = GlobVar.DEFAULT_PRIORITY;

            ProcessWrapper proc = new ProcessWrapper(processName);

            String message = processName + ": Process shut down";
            
            MessageWrapper mess = new MessageWrapper(message, category, priority);
            LogRelWrapper rel = new LogRelWrapper(mess.MessageId, proc.ProcessId);
        }

        /// <summary>
        /// Logs the change of a process' state.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        /// <param name="state">The process state.</param>
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

        /// <summary>
        /// converts a ProcessesState enum to a string representation.
        /// </summary>
        /// <param name="state">The ProcessState enum value.</param>
        /// <returns></returns>
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