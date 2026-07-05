# SegmentTreeConsole

A .NET 10 playground for a **generic lazy segment tree** with a concrete **min / max / sum** implementation. The tree supports range queries and range additive updates in `O(log n)` time, with aggressive optimizations (Native AOT, tiered PGO, server GC) for high-throughput workloads.

## Solution overview

| Project | Purpose |
|---------|---------|
| **SegmentTreeConsole** | Core library and demo console app |
| **SegmentTreeConsole.Tests** | NUnit correctness tests |
| **SegmentTreeConsole.Benchmark** | BenchmarkDotNet performance benchmarks |

---

## SegmentTreeConsole

The main project contains a reusable, strategy-based segment tree and one factory that wires it up for min/max/sum queries with lazy range updates.

### Core types

- **`GenericSegmentTree<TValue, TValueUpdate, TLazy>`** — Generic lazy segment tree. Behavior is supplied via constructor delegates: node creation, attribute updates, lazy propagation, child merging, and query result combination.
- **`ISegmentTreeNode<TValue, TLazy>`** — Node contract (range, attributes, lazy tags, leaf detection).
- **`ISegmentTreeFactory<TValue, TValueUpdate, TLazy>`** — Factory interface for building configured trees.

### Min / max / sum implementation (`Factory/`)

- **`MinMaxSumLazySegmentTreeFactory`** — Builds a tree over `int[][]` input where each element is a single-value array. Each query returns `[min, max, sum]` for the requested range. Range updates add a constant to every element in the range.
- **`MinMaxSumLazySegmentTreeNode`** / **`MinMaxSumLazySegmentTreeLeaf`** — Internal node and leaf types that store aggregated min, max, sum and propagate lazy add tags.

### Demo (`Program.cs`)

Runs a small interactive demo on 8 elements, then a large-scale perf smoke test (100M elements, 100 range updates) using `Stopwatch` and a no-GC region.

### Build notes

- Target: **.NET 10**, **x64**
- Release builds enable **Native AOT**, trimming, tiered PGO, and server GC

---

## SegmentTreeConsole.Tests

NUnit test suite validating the min/max/sum lazy segment tree:

- Single-element and range queries
- Range updates followed by range or point queries
- Updates outside queried ranges
- Multiple sequential updates
- Full-range update with partial query

Run tests:

```bash
dotnet test SegmentTreeConsole.Tests
```

---

## SegmentTreeConsole.Benchmark

BenchmarkDotNet benchmarks for the min/max/sum lazy segment tree at **N = 100,000,000** elements (seed `260504`, values in `[0, 999]`).

### Current benchmark configuration (source code)

| Benchmark | Range | Coverage |
|-----------|-------|----------|
| `QueryRangeTest` | `[3N/11, 9N/11]` | ~54.5% (~55M elements) |
| `UpdateTreeTest` | `[3N/11, 5N/11]`, add **+31**, reversed in `IterationCleanup` | ~18.2% (~18M elements) |

Run benchmarks (Release recommended):

```bash
dotnet run -c Release --project SegmentTreeConsole.Benchmark
```

Reports are written to `SegmentTreeConsole.Benchmark/BenchmarkDotNet.Artifacts/results/`.

---

## Benchmark results

Captured on **13–14 Jun 2026** with BenchmarkDotNet **v0.15.8**.

> **Stale update results:** The update benchmark was run on **14 Jun 2026 00:26**, but `MinMaxSumLazySegmentTreeUpdateBenchmark.cs` was not narrowed to `[3N/11, 5N/11]` until **14 Jun 2026 10:47**. The update numbers below therefore used the old **`[3N/11, 9N/11]`** range (~54.5%), not the current ~18.2% configuration. Re-run the benchmark for up-to-date update timings.

**Environment**

| | |
|---|---|
| OS | Windows 11 (10.0.26200) |
| CPU | AMD Ryzen 9 9900X @ 4.40 GHz (12 physical / 24 logical cores) |
| Runtime | .NET 10.0.5, X64 NativeAOT x86-64-v4 |
| GC | Non-concurrent Workstation |

### Query benchmark

Array size: **100M**. Query range: indices `[27,272,727 … 81,818,181]` (~54.5% of the array).

| Method | Mean | Error | StdDev |
|--------|-----:|------:|-------:|
| `QueryRangeTest` | **2.167 μs** | 0.0382 μs | 0.0357 μs |

A single min/max/sum range query over ~55M elements completes in about **2.2 microseconds**.

### Update benchmark *(stale — wider range than current source)*

Array size: **100M**. Range used at run time: `[27,272,727 … 81,818,181]` (`[3N/11, 9N/11]`, **~54.5%**). Add value: **+31** (undone each iteration).

| Method | Mean | Error | StdDev |
|--------|-----:|------:|-------:|
| `UpdateTreeTest` | **40.58 μs** | 2.587 μs | 7.627 μs |

~41 μs to add over ~55M elements with the old range. Current source targets ~18M elements (`[3N/11, 5N/11]`) — expect a lower time after re-running.

> **Note:** Update benchmark uses `InvocationCount=1`, `UnrollFactor=1` because each iteration mutates tree state; `IterationCleanup` reverses the update so iterations stay independent.

---

## Getting started

```bash
# Clone and build
dotnet build SegmentTreeConsole.sln -c Release

# Run demo
dotnet run --project SegmentTreeConsole -c Release

# Run tests
dotnet test

# Run benchmarks (long-running; needs substantial RAM for 100M-element tree)
dotnet run -c Release --project SegmentTreeConsole.Benchmark
```

## License

No license file is included; treat as a personal playground project unless otherwise specified.
