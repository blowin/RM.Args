namespace RM.Args.Tests;

public class ArgsTest
{
    [Fact]
    public void TestWithNoSchemaButWithOneArgument()
    {
        try
        {
            new Args("", new[] { "-x" });
        }
        catch (UnexpectedArgumentException e)
        {
            Assert.Equal('x', e.ErrorArgumentId);
        }
    }

    [Fact]
    public void TestWithNoSchemaButWithMultipleArguments()
    {
        try
        {
            new Args("", new[] { "-x", "-y" });
        }
        catch (UnexpectedArgumentException e)
        {
            Assert.Equal('x', e.ErrorArgumentId);
        }
    }

    [Fact]
    public void TestNonLetterSchema()
    {
        try
        {
            new Args("*", new String[] { });
        }
        catch (InvalidArgumentNameException e)
        {
            Assert.Equal('*', e.ErrorArgumentId);
        }
    }

    [Fact]
    public void TestInvalidArgumentFormat()
    {
        try
        {
            new Args("f~", new String[] { });
        }
        catch (InvalidArgsFormatException e)
        {
            Assert.Equal('f', e.ErrorArgumentId);
        }
    }

    [Fact]
    public void TestSimpleBooleanPresent()
    {
        var args = new Args("x", new[] { "-x", "true" });
        Assert.True(args.Get<bool>('x'));
    }

    [Fact]
    public void TestSimpleStringPresent()
    {
        var args = new Args("x*", new[] { "-x", "param" });
        Assert.True(args.Has('x'));
        Assert.Equal("param", args.Get<string>('x'));
    }

    [Fact]
    public void TestMissingStringArgument()
    {
        try
        {
            new Args("x*", new[] { "-x" });
        }
        catch (MissingArgsException e)
        {
            Assert.Equal('x', e.ErrorArgumentId);
        }
    }

    [Fact]
    public void TestSpacesInFormat()
    {
        var args = new Args("x, y", new[] { "-xy", "true" });
        Assert.True(args.Has('x'));
        Assert.True(args.Has('y'));
    }

    [Fact]
    public void TestSimpleIntPresent()
    {
        var args = new Args("x#", new[] { "-x", "42" });
        Assert.True(args.Has('x'));
        Assert.Equal(42, args.Get<int>('x'));
    }

    [Fact]
    public void TestInvalidInteger()
    {
        try
        {
            new Args("x#", new[] { "-x", "Forty two" });
        }
        catch (InvalidArgsException e)
        {
            Assert.Equal('x', e.ErrorArgumentId);
            Assert.Equal("Forty two", e.ErrorParameter);
        }
    }

    [Fact]
    public void TestMissingInteger()
    {
        try
        {
            new Args("x#", new[] { "-x" });
        }
        catch (MissingArgsException e)
        {
            Assert.Equal('x', e.ErrorArgumentId);
        }
    }

    [Fact]
    public void TestSimpleDoublePresent()
    {
        var args = new Args("x##", new[] { "-x", "42,3" });
        Assert.True(args.Has('x'));
        Assert.Equal(42.3, args.Get<double>('x'), .001);
    }

    [Fact]
    public void TestInvalidDouble()
    {
        try
        {
            new Args("x##", new[] { "-x", "Forty two" });
        }
        catch (InvalidArgsException e)
        {
            Assert.Equal('x', e.ErrorArgumentId);
            Assert.Equal("Forty two", e.ErrorParameter);
        }
    }

    [Fact]
    public void TestMissingDouble()
    {
        try
        {
            new Args("x##", new[] { "-x" });
        }
        catch (MissingArgsException e)
        {
            Assert.Equal('x', e.ErrorArgumentId);
        }
    }
    
    [Fact]
    public void TestExtraArguments()
    {
        var args = new Args("x,y*", new[] { "-x", "true", "-y", "alpha" });
        Assert.True(args.Get<bool>('x'));
        Assert.Equal("alpha", args.Get<string>('y'));
    }

    [Fact]
    public void TestExtraArgumentsThatLookLikeFlags()
    {
        var args = new Args("x,y", new[] { "-x", "true", "-y", "true" });
        Assert.True(args.Has('x'));
        Assert.True(args.Has('y'));
        Assert.True(args.Get<bool>('x'));
        Assert.True(args.Get<bool>('y'));
    }
}