﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Microsoft.AspNetCore.Blazor.Build.Test
{
    public class DesignTimeCodeGenerationRazorIntegrationTest : RazorIntegrationTestBase
    {
        internal override bool DesignTime => true;

        internal override bool UseTwoPhaseCompilation => true;
        
        [Fact]
        public void CodeGeneration_ChildComponent_WithParameters()
        {
            // Arrange
            AdditionalSyntaxTrees.Add(CSharpSyntaxTree.ParseText(@"
using Microsoft.AspNetCore.Blazor.Components;

namespace Test
{
    public class SomeType
    {
    }

    public class MyComponent : BlazorComponent
    {
        public int IntProperty { get; set; }
        public bool BoolProperty { get; set; }
        public string StringProperty { get; set; }
        public SomeType ObjectProperty { get; set; }
    }
}
"));

            // Act
            var generated = CompileToCSharp(@"
@addTagHelper *, TestAssembly
<MyComponent 
    IntProperty=""123""
    BoolProperty=""true""
    StringProperty=""My string""
    ObjectProperty=""new SomeType()""/>");

            // Assert
            CompileToAssembly(generated);

            AssertSourceEquals(@"
// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class TestComponent : Microsoft.AspNetCore.Blazor.Components.BlazorComponent
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((System.Action)(() => {
global::System.Object __typeHelper = ""*, TestAssembly"";
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Blazor.RenderTree.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            __o = 
#line 3 ""x:\dir\subdir\Test\TestComponent.cshtml""
                 123

#line default
#line hidden
            ;
            __o = 
#line 4 ""x:\dir\subdir\Test\TestComponent.cshtml""
                  true

#line default
#line hidden
            ;
            __o = 
#line 6 ""x:\dir\subdir\Test\TestComponent.cshtml""
                    new SomeType()

#line default
#line hidden
            ;
            builder.AddAttribute(-1, ""ChildContent"", (Microsoft.AspNetCore.Blazor.RenderFragment)((builder2) => {
            }
            ));
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591

", generated);
        }

        [Fact]
        public void CodeGeneration_ChildComponent_WithExplicitStringParameter()
        {
            // Arrange
            AdditionalSyntaxTrees.Add(CSharpSyntaxTree.ParseText(@"
using Microsoft.AspNetCore.Blazor.Components;

namespace Test
{
    public class MyComponent : BlazorComponent
    {
        public string StringProperty { get; set; }
    }
}
"));

            // Act
            var generated = CompileToCSharp(@"
@addTagHelper *, TestAssembly
<MyComponent StringProperty=""@(42.ToString())"" />");

            // Assert
            CompileToAssembly(generated);

            AssertSourceEquals(@"
// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class TestComponent : Microsoft.AspNetCore.Blazor.Components.BlazorComponent
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((System.Action)(() => {
global::System.Object __typeHelper = ""*, TestAssembly"";
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Blazor.RenderTree.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            __o = 
#line 2 ""x:\dir\subdir\Test\TestComponent.cshtml""
                               42.ToString()

#line default
#line hidden
            ;
            builder.AddAttribute(-1, ""ChildContent"", (Microsoft.AspNetCore.Blazor.RenderFragment)((builder2) => {
            }
            ));
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
", generated);
        }

        [Fact]
        public void CodeGeneration_ChildComponent_WithNonPropertyAttributes()
        {
            // Arrange
            AdditionalSyntaxTrees.Add(CSharpSyntaxTree.ParseText(@"
using Microsoft.AspNetCore.Blazor.Components;

namespace Test
{
    public class MyComponent : BlazorComponent
    {
    }
}
"));

            // Act
            var generated = CompileToCSharp(@"
@addTagHelper *, TestAssembly
<MyComponent some-attribute=""foo"" another-attribute=""@(43.ToString())""/>");

            // Assert
            CompileToAssembly(generated);

            AssertSourceEquals(@"
// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class TestComponent : Microsoft.AspNetCore.Blazor.Components.BlazorComponent
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((System.Action)(() => {
global::System.Object __typeHelper = ""*, TestAssembly"";
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Blazor.RenderTree.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            __o = 
#line 2 ""x:\dir\subdir\Test\TestComponent.cshtml""
                                                       43.ToString()

#line default
#line hidden
            ;
            builder.AddAttribute(-1, ""ChildContent"", (Microsoft.AspNetCore.Blazor.RenderFragment)((builder2) => {
            }
            ));
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
", generated);
        }

        [Fact]
        public void CodeGeneration_ChildComponent_WithLambdaEventHandler()
        {
            // Arrange
            AdditionalSyntaxTrees.Add(CSharpSyntaxTree.ParseText(@"
using System;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Test
{
    public class MyComponent : BlazorComponent
    {
        public UIEventHandler OnClick { get; set; }
    }
}
"));

            // Act
            var generated = CompileToCSharp(@"
@addTagHelper *, TestAssembly
<MyComponent OnClick=""Increment()""/>

@functions {
    private int counter;
    private void Increment() {
        counter++;
    }
}");

            // Assert
            CompileToAssembly(generated);

            AssertSourceEquals(@"
// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class TestComponent : Microsoft.AspNetCore.Blazor.Components.BlazorComponent
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((System.Action)(() => {
global::System.Object __typeHelper = ""*, TestAssembly"";
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Blazor.RenderTree.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            __o = new Microsoft.AspNetCore.Blazor.UIEventHandler((eventArgs) => 
#line 2 ""x:\dir\subdir\Test\TestComponent.cshtml""
                      Increment()

#line default
#line hidden
            );
            builder.AddAttribute(-1, ""ChildContent"", (Microsoft.AspNetCore.Blazor.RenderFragment)((builder2) => {
            }
            ));
        }
        #pragma warning restore 1998
#line 4 ""x:\dir\subdir\Test\TestComponent.cshtml""
            
    private int counter;
    private void Increment() {
        counter++;
    }

#line default
#line hidden
    }
}
#pragma warning restore 1591

", generated);
        }

        [Fact]
        public void CodeGeneration_ChildComponent_WithExplicitEventHandler()
        {
            // Arrange
            AdditionalSyntaxTrees.Add(CSharpSyntaxTree.ParseText(@"
using System;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Test
{
    public class MyComponent : BlazorComponent
    {
        public UIEventHandler OnClick { get; set; }
    }
}
"));

            // Act
            var generated = CompileToCSharp(@"
@addTagHelper *, TestAssembly
@using Microsoft.AspNetCore.Blazor
<MyComponent OnClick=""@Increment""/>

@functions {
    private int counter;
    private void Increment(UIEventArgs e) {
        counter++;
    }
}");

            // Assert
            CompileToAssembly(generated);

            AssertSourceEquals(@"
// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#line 2 ""x:\dir\subdir\Test\TestComponent.cshtml""
using Microsoft.AspNetCore.Blazor;

#line default
#line hidden
    public class TestComponent : Microsoft.AspNetCore.Blazor.Components.BlazorComponent
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((System.Action)(() => {
global::System.Object __typeHelper = ""*, TestAssembly"";
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Blazor.RenderTree.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            __o = new Microsoft.AspNetCore.Blazor.UIEventHandler(
#line 3 ""x:\dir\subdir\Test\TestComponent.cshtml""
                       Increment

#line default
#line hidden
            );
            builder.AddAttribute(-1, ""ChildContent"", (Microsoft.AspNetCore.Blazor.RenderFragment)((builder2) => {
            }
            ));
        }
        #pragma warning restore 1998
#line 5 ""x:\dir\subdir\Test\TestComponent.cshtml""
            
    private int counter;
    private void Increment(UIEventArgs e) {
        counter++;
    }

#line default
#line hidden
    }
}
#pragma warning restore 1591

", generated);
        }

        [Fact]
        public void CodeGeneration_ChildComponent_WithChildContent()
        {
            // Arrange
            AdditionalSyntaxTrees.Add(CSharpSyntaxTree.ParseText(@"
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Test
{
    public class MyComponent : BlazorComponent
    {
        public string MyAttr { get; set; }

        public RenderFragment ChildContent { get; set; }
    }
}
"));

            // Act
            var generated = CompileToCSharp(@"
@addTagHelper *, TestAssembly
<MyComponent MyAttr=""abc"">Some text<some-child a='1'>Nested text</some-child></MyComponent>");

            // Assert
            CompileToAssembly(generated);

            AssertSourceEquals(@"
// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class TestComponent : Microsoft.AspNetCore.Blazor.Components.BlazorComponent
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        ((System.Action)(() => {
global::System.Object __typeHelper = ""*, TestAssembly"";
        }
        ))();
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Blazor.RenderTree.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.AddAttribute(-1, ""ChildContent"", (Microsoft.AspNetCore.Blazor.RenderFragment)((builder2) => {
            }
            ));
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
", generated);
        }
    }
}