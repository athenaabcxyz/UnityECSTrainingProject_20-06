﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Unity.Entities.SourceGen.Common;
using Unity.Entities.SourceGen.SystemGenerator.SystemAPI.Query;
using Unity.Entities.SourceGen.SystemGenerator;
using VerifyCS =
    Unity.Entities.SourceGenerators.Test.CSharpSourceGeneratorVerifier<
        Unity.Entities.SourceGen.SystemGenerator.SystemGenerator>;

namespace Unity.Entities.SourceGenerators;

[TestClass]
public class ForEachErrorTests
{
    [TestMethod]
    public async Task SGFE002_ForEachIterationThroughAspectQuery_InPropertyAccessor()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;
            {|#0:partial struct TranslationSystem : ISystem
            {
                public int Translation
                {
                    get
                    {
                        foreach (var translation in SystemAPI.Query<RefRW<Translation>>()){}
                        return 3;
                    }
                }

                public void OnCreate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state) { }

                public void OnUpdate(ref SystemState state) { }
            }|}";
        var expected = VerifyCS.CompilerError(nameof(IdiomaticCSharpForEachCompilerMessages.SGFE002)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGQC001_QueryingUnsupportedType()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;
            struct NotAComponent { }

            partial struct TranslationSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state)
                {
                    foreach (var aspect in {|#0:SystemAPI.Query<RefRW<Translation>>().WithNone<NotAComponent>()|})
                    {
                    }
                }

                public void OnUpdate(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(QueryConstructionErrors.SGQC001)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGQC004_SameTypeSpecifiedInWithNone_AndWithAll()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct TranslationSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state)
                {
                    foreach (var aspect in {|#0:SystemAPI.Query<RefRW<Translation>>().WithNone<Translation>().WithAll<Translation>()|})
                    {
                    }
                }

                public void OnUpdate(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(QueryConstructionErrors.SGQC004)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGQC004_SameTypeSpecifiedInWithNone_AndWithAny()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct TranslationSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state)
                {
                    foreach (var aspect in {|#0:SystemAPI.Query<RefRW<Translation>>().WithNone<Translation>().WithAny<Translation>()|})
                    {
                    }
                }

                public void OnUpdate(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(QueryConstructionErrors.SGQC004)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGQC004_SameTypeSpecifiedInWithAll_AndWithAny()
    {
        const string source =@"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct TranslationSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state)
                {
                    foreach (var aspect in {|#0:SystemAPI.Query<RefRW<Translation>>().WithAll<Translation>().WithAny<Translation>()|})
                    {
                    }
                }

                public void OnUpdate(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(QueryConstructionErrors.SGQC004)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGQC004_QueriedAspectTypeSpecifiedInWithNone()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct TranslationSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state)
                {
                    foreach (var aspect in {|#0:SystemAPI.Query<RefRW<Translation>>().WithNone<Translation>()|})
                    {
                    }
                }

                public void OnUpdate(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(QueryConstructionErrors.SGQC004)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGQC004_QueriedAspectTypeSpecifiedInWithAny()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct TranslationSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state)
                {
                    foreach (var translation in {|#0:SystemAPI.Query<RefRW<Translation>>().WithAny<Translation>()|})
                    {
                    }
                }

                public void OnUpdate(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(QueryConstructionErrors.SGQC004)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGSG0002_MethodWithoutSystemStateParameter()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }
                public void OnUpdate(ref SystemState state)
                {
                }
                public void MethodWithoutSystemStateParameter()
                {
                    foreach (var translation in {|#0:SystemAPI.Query<RefRW<Translation>>()|})
                    {
                    }
                }
                public void OnDestroy(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(SystemGeneratorErrors.SGSG0002)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGFE003_TooManyChangeFilters()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }
                public void OnUpdate(ref SystemState state)
                {
                    foreach (var aspect in {|#0:SystemAPI.Query<RefRO<EcsTestData>>()
                    .WithChangeFilter<EcsTestData2, EcsTestData3>()
                    .WithChangeFilter<EcsTestData4>()|}) {}
                }
                public void OnDestroy(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(IdiomaticCSharpForEachCompilerMessages.SGFE003)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGFE007_TooManySharedComponentChangeFilters()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }
                public void OnUpdate(ref SystemState state)
                {
                    foreach (var component in {|#0:SystemAPI.Query<RefRO<EcsTestData>>()
                        .WithSharedComponentFilter<EcsTestData2, EcsTestData3>()
                        .WithSharedComponentFilter<EcsTestData4>()|}) {}
                }
                public void OnDestroy(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(IdiomaticCSharpForEachCompilerMessages.SGFE007)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGFE008_TooManyEntityQueryOptionsArguments()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnUpdate(ref SystemState state)
                {
                    foreach (var component in {|#0:SystemAPI.Query<RefRO<EcsTestData>>().WithOptions(EntityQueryOptions.Default).WithOptions(EntityQueryOptions.FilterWriteGroup)|})
                    {}
                }

                public void OnDestroy(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(IdiomaticCSharpForEachCompilerMessages.SGFE008)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGFE001_SystemAPIQueryInvoked_WithoutForEach()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;

            partial class CharacterMovementSystem : SystemBase
            {
                protected override void OnUpdate() { }

                public QueryEnumerable<RefRO<EcsTestData>> GetEntitiesWithEcsTestData() => {|#0:SystemAPI.Query<RefRO<EcsTestData>>()|};
            }";
        var expected = VerifyCS.CompilerError(nameof(IdiomaticCSharpForEachCompilerMessages.SGFE001)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGFE009_ValueTypeComponentDataInForEachIteration()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnUpdate(ref SystemState state)
                {
                    foreach (var component in {|#0:SystemAPI.Query<EcsTestData>()|})
                    { }
                }

                public void OnDestroy(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerInfo(nameof(IdiomaticCSharpForEachCompilerMessages.SGFE009)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task SGFE010_ValueTypeComponentDataInForEachIteration()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnUpdate(ref SystemState state)
                {
                    foreach (var component in SystemAPI.Query<{|#0:RefRO<GenericComponentData<EcsTestData>>|}>())
                    { }
                }

                public void OnDestroy(ref SystemState state) { }
            }";
        var expected = VerifyCS.CompilerError(nameof(IdiomaticCSharpForEachCompilerMessages.SGFE010)).WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }

    [TestMethod]
    public async Task ForEachIteration_WithDataWithCycle()
    {
        const string source = @"
            using Unity.Entities;
            using Unity.Entities.Tests;

            public struct CycleData : IComponentData
            {
                CycleData {|#0:Value|};
            }

            public partial class SomeSystem : SystemBase {
                protected override void OnUpdate() {
                    foreach (var cycleData in SystemAPI.Query<CycleData>()) {}
                }
            }";

        var expected = VerifyCS.CompilerError("CS0523").WithLocation(0);
        await VerifyCS.VerifySourceGeneratorAsync(source, expected);
    }
}
