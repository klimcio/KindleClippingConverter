using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("KindleClippingTools.Tests")]
namespace KindleClippingTools.Logic
{
    internal static class ClippingExtensions
    {
        internal static void BackupKindleClippings(
            this IEnumerable<IGrouping<string,Clipping>> clippings, 
            string targetDirectory)
        {
            foreach (var group in clippings)
            {
                group.BackupGroup(targetDirectory);
            }
        }

        internal static void BackupGroup(this IGrouping<string, Clipping> group, string targetDirectory)
        {
            var bookTitle = group.Key.ConvertStringToFolderName();
            var dirInfo = Directory.CreateDirectory(Path.Combine(targetDirectory, bookTitle));

            var highlights = group.Where(x => x.Type == ClippingType.Highlight).ToList();
            var notes = group.Where(x => x.Type == ClippingType.Note).ToList();

            foreach (var clipping in highlights)
            {
                string path = PrepareNewFilePath(dirInfo, clipping);

                if (File.Exists(path))
                {
                    clipping.AppendToFile(path);
                    continue;
                }

                BackupClipping(bookTitle, clipping, path);

                var correspondingNotes = notes.GetCorrespondingNotes(clipping);

                foreach (var foundNote in correspondingNotes)
                {
                    foundNote.AppendToFile(path);
                    notes.Remove(foundNote);
                }
            }

            foreach (var clipping in notes)
            {
                var path = PrepareNewFilePath(dirInfo, clipping);

                BackupClipping(bookTitle, clipping, path);
            }
        }

        private static string PrepareNewFilePath(DirectoryInfo dirInfo, Clipping clipping)
        {
            return Path.Combine(
                dirInfo.FullName,
                clipping.TempFileName
            );
        }

        internal static List<Clipping> GetCorrespondingNotes(this List<Clipping> notes, Clipping highlight)
        {
            var returnList = notes
                .Where(x => x.PageNumber == highlight.PageNumber)
                .Where(x => DoTheseClippingsCorrespond(x, highlight))
                .ToList();

            return returnList;
        }

        private static bool DoTheseClippingsCorrespond(this Clipping note, Clipping highlight)
        {
            return note.LocationStart == highlight.LocationStart
                || note.LocationStart == highlight.LocationStart + 1
                || note.LocationStart == highlight.LocationStart + 2;
        }

        private static void BackupClipping(string bookTitle, Clipping clipping, string path)
        {
            var builder = new StringBuilder();

            builder.AddBookReference(bookTitle);
            builder.AddClippingData(clipping.CreatedOn);
            builder.AppendLine();

            builder.AppendContent(clipping);

            File.WriteAllText(path, builder.ToString());
        }

        internal static void AppendToFile(this Clipping clipping, string path)
        {
            var toBeAppended = $"{(clipping.IsHighlight ? "> " : "")} {clipping.Content}";
            File.AppendAllText(path, toBeAppended);
        }
    }
}
