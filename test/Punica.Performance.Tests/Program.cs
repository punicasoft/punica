// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Punica.Performance.Tests.Punica.Linq;

Console.WriteLine("Hello, World!");


BenchmarkRunner.Run<DynamicTests>();