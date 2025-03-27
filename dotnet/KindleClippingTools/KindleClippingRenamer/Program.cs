internal class Program
{
    private static void Main(string[] args)
    {
        var rootFolder = args[0];
        var folders = Directory.GetDirectories(rootFolder);

        foreach(var folder in folders)
        {
            var folderName = Path.GetFileName(folder);

            var newFolderName = folderName.Replace("(", "").Split(' ')
                .Select(x => x[0])
                .Aggregate("", (acc, x) => acc + x)
                .Replace("(", "");

            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).Split("_");
                var type = fileName[0];
                var name = string.Join("_", fileName[1..]).Replace(".md", "");
                var newFileName = $"{newFolderName}_{name}_{type}.md";

                // rename every file in the folder to the new name
                File.Move(file, Path.Combine(folder, newFileName));
                //Console.WriteLine(newFileName);
            }
        }
    }
}