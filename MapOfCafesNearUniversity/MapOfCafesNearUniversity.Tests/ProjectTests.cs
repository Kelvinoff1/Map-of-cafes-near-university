using Xunit;
using MapOfCafesNearUniversity.Models;
using MapOfCafesNearUniversity.DTO;
using System.Collections.Generic;

namespace MapOfCafesNearUniversity.Tests
{
    public class ProjectTests
    {
        [Fact]
        public void Cafe_HasValidProperties()
        {
            var cafe = new Cafe
            {
                Name = "Student Coffee",
                Latitude = 50.4501,
                Longitude = 30.5234,
                PopupContent = "<b>Student Coffee</b>"
            };

            Assert.Equal("Student Coffee", cafe.Name);
            Assert.True(cafe.Latitude > 0);
            Assert.True(cafe.Longitude > 0);
            Assert.Contains("Coffee", cafe.PopupContent);
        }

        [Fact]
        public void OverpassResponse_CreatesElementsList()
        {
            var response = new OverpassResponse
            {
                Elements = new List<Element>
                {
                    new Element
                    {
                        Latitude = 50.45,
                        Longitude = 30.52,
                        Tags = new Tags { Name = "Cafe A" }
                    },
                    new Element
                    {
                        Latitude = 50.46,
                        Longitude = 30.53,
                        Tags = new Tags { Name = "Cafe B" }
                    }
                }
            };

            var count = response.Elements.Count;

            Assert.Equal(2, count);
            Assert.Equal("Cafe A", response.Elements[0].Tags!.Name);
            Assert.Equal("Cafe B", response.Elements[1].Tags!.Name);
        }
    }
}

