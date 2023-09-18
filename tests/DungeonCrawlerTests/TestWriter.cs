using System.Text;
using Xunit.Abstractions;

namespace Tests;

public class TestWriter: TextWriter {
    
    StringBuilder cache = new();
    public ITestOutputHelper OutputHelper { get; }

    public override Encoding Encoding => Encoding.ASCII;

    public TestWriter(ITestOutputHelper outputHelper){
        OutputHelper = outputHelper;
    }

    public override void Write(char value){
        if(value == '\n'){
            OutputHelper.WriteLine(cache.ToString());
        } else {
            cache.Append(value);
        }
    } 
    public override void Flush(){
        if(cache.Length == 0) return;
        OutputHelper.WriteLine(cache.ToString());
        cache.Clear();
    }

}