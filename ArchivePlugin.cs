using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace SimpleIntro
{
    public class ArchivePlugin
    {
        [KernelFunction("archive_data")]
        [Description("Saves the data to a file on your computer.")]
        public async Task WriteData(Kernel kernel, string fileName, string data)
        {
            await File.WriteAllTextAsync($"/Users/leonardowildt/Documents/LeosCode/semanticKernel/simpleintro/{fileName}.txt", data);
        }
    }
}