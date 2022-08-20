﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BeastLearn.Application.Jobs
{
    public class JobSchedule
    {
        public Type JobType { get; set; }
        public string CronExpression { get; set; }
        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }
    }
}
