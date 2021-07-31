using FluentAssertions;
using KindleClippingTools.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KindleClippingTools.Tests
{
    public class TestingParsingClippingFileTests
    {
        private string OneClippingFile => $"{Environment.CurrentDirectory}\\TestFiles\\OneClippingFile.txt";
        private string MultipleClippingFile => $"{Environment.CurrentDirectory}\\TestFiles\\MultipleClippingFiles.txt";

        private List<Clipping> ParseFile(string filePath)
        {
            var sut = new KindleClippingParser();

            var results = sut.ParseFile(filePath);
            return results;
        }

        [Fact]
        public void Testing_when_one_clipping_file_will_return_list_of_one_element()
        {
            List<Clipping> results = ParseFile(OneClippingFile);

            results.Count.Should().Be(1);

            var clipping = results.First();
            clipping.Title.Should().Be("Pismo Święte Starego Testamentu (Autor zbiorowy)");
            clipping.Type.Should().Be(ClippingType.Highlight);
            clipping.PageNumber.Should().Be(6);
            clipping.LocationStart.Should().Be(61);
            clipping.LocationEnd.Should().Be(62);
            clipping.CreatedOn.Should().Be(new DateTime(2014, 9, 9, 21, 41, 6));
            clipping.Content.Should().Be("„Tora”, czyli „Prawo”, wskazuje na ich treść: przepisy prawa, przekazanego Mojżeszowi (stąd także nazwa „Prawo Mojżeszowe”) dla całego Izraela, zajmują najwięcej miejsca.");
        }


        // TODO: Test with multiple line content

        [Fact]
        public void Testing_multiple_clipping_file_will_return_list_of_elements()
        {
            List<Clipping> results = ParseFile(MultipleClippingFile);

            results.Count.Should().Be(4);

            results.Count(x => x.Type == ClippingType.Bookmark).Should().Be(1);
            results.Count(x => x.Type == ClippingType.Note).Should().Be(1);
            results.Count(x => x.Type == ClippingType.Highlight).Should().Be(2);

            var bookmark = results.First(x => x.Type == ClippingType.Bookmark);
            bookmark.Should().NotBeNull();
            bookmark.Title.Should().Be("Pismo Święte Starego Testamentu (Autor zbiorowy)");
            bookmark.PageNumber.Should().Be(44);
            bookmark.LocationStart.Should().Be(622);
            bookmark.LocationEnd.HasValue.Should().BeFalse();
            bookmark.CreatedOn.Should().Be(new DateTime(2015, 4, 3, 9, 15, 14));
            bookmark.Content.Should().BeEmpty();

            var note = results.First(x => x.Type == ClippingType.Note);
            note.Should().NotBeNull();
            note.Title.Should().Be("﻿Książę (Niccolo Machiavelli)");
            note.PageNumber.Should().Be(3);
            note.LocationStart.Should().Be(35);
            note.LocationEnd.HasValue.Should().BeFalse();
            note.CreatedOn.Should().Be(new DateTime(2014, 9, 10, 17, 12, 8));
            note.Content.Should().Be("gdzie?");

            var highlights = results.Where(x => x.Type == ClippingType.Highlight);
            var highlight1 = highlights.ElementAt(0);
            highlight1.Title.Should().Be("﻿Książę (Niccolo Machiavelli)");
            highlight1.PageNumber.Should().Be(3);
            highlight1.LocationStart.Should().Be(35);
            highlight1.LocationEnd.Should().Be(35);
            highlight1.CreatedOn.Should().Be(new DateTime(2014, 9, 10, 17, 11, 40));
            highlight1.Content.Should().Be("Nie będę wdawał się w rozprawę o republikach, gdyż gdzie indziej omówiłem je obszernie.");

            var highlight2 = highlights.ElementAt(1);
            highlight2.Title.Should().Be("﻿Książę (Niccolo Machiavelli)");
            highlight2.PageNumber.Should().Be(4);
            highlight2.LocationStart.Should().Be(50);
            highlight2.LocationEnd.Should().Be(53);
            highlight2.CreatedOn.Should().Be(new DateTime(2014, 9, 10, 17, 16, 43));
            highlight2.Content.Should().Be("przewroty w nim rodzą się przede wszystkim z tej naturalnej przyczyny, która istnieje w każdym nowym księstwie. Ludzie bowiem chętnie zmieniają pana w tej nadziei, że poprawią swój los, i ta wiara daje im przeciwko panującemu broń w rękę - w czym zawodzą się, przekonawszy się przez doświadczenie, że tylko pogorszyli swe położenie.");
        }

        // TODO: Test highlight with attached note
    }
}
