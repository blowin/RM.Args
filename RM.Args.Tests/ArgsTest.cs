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
        catch (ArgsException e)
        {
            Assert.Equal(ArgsException.ErrorCode.UnexpectedArgument, e.GetErrorCode());
            Assert.Equal('x', e.GetErrorArgumentId());
        }
    }

    [Fact]
    public void TestWithNoSchemaButWithMultipleArguments()
    {
        try
        {
            new Args("", new[] { "-x", "-y" });
        }
        catch (ArgsException e)
        {
            Assert.Equal(ArgsException.ErrorCode.UnexpectedArgument, e.GetErrorCode());
            Assert.Equal('x', e.GetErrorArgumentId());
        }
    }

    [Fact]
    public void TestNonLetterSchema()
    {
        try
        {
            new Args("*", new String[] { });
        }
        catch (ArgsException e)
        {
            Assert.Equal(ArgsException.ErrorCode.InvalidArgumentName, e.GetErrorCode());
            Assert.Equal('*', e.GetErrorArgumentId());
        }
    }

    [Fact]
    public void TestInvalidArgumentFormat()
    {
        try
        {
            new Args("f~", new String[] { });
        }
        catch (ArgsException e)
        {
            Assert.Equal(ArgsException.ErrorCode.InvalidFormat, e.GetErrorCode());
            Assert.Equal('f', e.GetErrorArgumentId());
        }
    }

    [Fact]
    public void TestSimpleBooleanPresent()
    {
        var args = new Args("x", new[] { "-x" });
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
        catch (ArgsException e)
        {
            Assert.Equal(ArgsException.ErrorCode.MissingString, e.GetErrorCode());
            Assert.Equal('x', e.GetErrorArgumentId());
        }
    }

    [Fact]
    public void TestSpacesInFormat()
    {
        var args = new Args("x, y", new[] { "-xy" });
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
        catch (ArgsException e)
        {
            Assert.Equal(ArgsException.ErrorCode.InvalidInteger, e.GetErrorCode());
            Assert.Equal('x', e.GetErrorArgumentId());
            Assert.Equal("Forty two", e.GetErrorParameter());
        }
    }

    [Fact]
    public void TestMissingInteger()
    {
        try
        {
            new Args("x#", new[] { "-x" });
        }
        catch (ArgsException e)
        {
            Assert.Equal(ArgsException.ErrorCode.MissingInteger, e.GetErrorCode());
            Assert.Equal('x', e.GetErrorArgumentId());
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
        catch (ArgsException e)
        {
            Assert.Equal(ArgsException.ErrorCode.InvalidDouble, e.GetErrorCode());
            Assert.Equal('x', e.GetErrorArgumentId());
            Assert.Equal("Forty two", e.GetErrorParameter());
        }
    }

    [Fact]
    public void TestMissingDouble()
    {
        try
        {
            new Args("x##", new[] { "-x" });
        }
        catch (ArgsException e)
        {
            Assert.Equal(ArgsException.ErrorCode.MissingDouble, e.GetErrorCode());
            Assert.Equal('x', e.GetErrorArgumentId());
        }
    }
    
    [Fact]
    public void TestExtraArguments()
    {
        var args = new Args("x,y*", new[] { "-x", "-y", "alpha", "beta" });
        Assert.True(args.Get<bool>('x'));
        Assert.Equal("alpha", args.Get<string>('y'));
    }

    [Fact]
    public void TestExtraArgumentsThatLookLikeFlags()
    {
        var args = new Args("x,y", new[] { "-x", "alpha", "-y", "beta" });
        Assert.True(args.Has('x'));
        Assert.True(args.Has('y'));
        Assert.True(args.Get<bool>('x'));
        Assert.True(args.Get<bool>('y'));
    }
}