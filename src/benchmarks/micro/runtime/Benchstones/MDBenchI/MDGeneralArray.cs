// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
//

using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using MicroBenchmarks;

namespace Benchstone.MDBenchI
{
[BenchmarkCategory(Categories.Runtime, Categories.Benchstones, Categories.JIT, Categories.MDBenchI)]
public class MDGeneralArray
{
    public const int Iterations = 5000;

    static void Initialize(int[,,] s) {
        for (int i = s.GetLowerBound(0); i <= s.GetUpperBound(0); i++) {
            for (int j = s.GetLowerBound(1); j <= s.GetUpperBound(1); j++) {
                for (int k = s.GetLowerBound(2); k <= s.GetUpperBound(2); k++) {
                    s[i,j,k] = (2 * i) - (3 * j) + (5 * k);
                }
            }
        }
    }

    static bool VerifyCopy(int[,,] s, int[,,] d) {
        for (int i = s.GetLowerBound(0); i <= s.GetUpperBound(0); i++) {
            for (int j = s.GetLowerBound(1); j <= s.GetUpperBound(1); j++) {
                for (int k = s.GetLowerBound(2); k <= s.GetUpperBound(2); k++) {
                    if (s[i,j,k] != d[i,j,k]) {
                        return false;
                    }
                }
            }
        }

        return true;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    static bool Bench(int loop, int[,,] s, int[,,] d) {

        Initialize(s);

        for (; loop != 0; loop--) {
            for (int i = s.GetLowerBound(0); i <= s.GetUpperBound(0); i++) {
                for (int j = s.GetLowerBound(1); j <= s.GetUpperBound(1); j++) {
                    for (int k = s.GetLowerBound(2); k <= s.GetUpperBound(2); k++) {
                        d[i,j,k] = s[i,j,k];
                    }
                }
            }
        }

        bool result = VerifyCopy(s, d);

        return result;
    }

    [Benchmark(Description = nameof(MDGeneralArray))]
    public bool Test() {
        int[,,] s = new int[10, 10, 10];
        int[,,] d = new int[10, 10, 10];
        return Bench(Iterations, s, d);
    }

    [Benchmark(Description = "MDGeneralArray2")]
    public bool Test2() {
        int[] lengths = new int[3] { 10, 10, 10 };
        int[] lowerBounds = new int[3] { -5, 0, 5 };
        int[,,] s = (int[,,])System.Array.CreateInstance(typeof(int), lengths, lowerBounds);
        int[,,] d = (int[,,])System.Array.CreateInstance(typeof(int), lengths, lowerBounds);
        return Bench(Iterations, s, d);
    }
}
}
