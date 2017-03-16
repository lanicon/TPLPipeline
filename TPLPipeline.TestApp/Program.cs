﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TPLPipeline.TestApp
{
	public interface IJobElementStartData { string Url { get; } }
	struct Website : IJobElementStartData { public string Url { get; set; } };
	struct Thumbnail : IJobElementStartData { public string Url { get; set; } };

	class Program
	{
		static void Main(string[] args)
		{
			var r = new Random();
			var jobList = new List<Job>();

			var pipeline = new Pipeline();

			for (int l = 0; l < 2; l++)
			{
				var job = new Job();

				for (int i = 0; i < 10; i++)
				{
					job.AddData(new Website { Url = "http://thomas-ict.nl" });
				}
				
				job.AddData(new Thumbnail { Url = "http://thomas-ict.nl/logo-groot.png" });

				jobList.Add(job);
			}

			var tasks = new List<Task>();

			foreach (var job in jobList)
			{
				tasks.Add(pipeline.PostAsync(job));
			}

			Task.WaitAll(tasks.ToArray());

			foreach (var job in jobList)
			{
				foreach (var el in ((IPipelineJob)job).Elements())
				{
					Console.WriteLine($"TRACE: {el.CompletedStepName} {(el.Disabled ? "MERGED" : "COMPLETED")}");
				}
			}

			Console.WriteLine("*** Done ***");
			Console.ReadLine();
		}
	}
}
