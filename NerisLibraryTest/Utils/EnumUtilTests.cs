using FluentAssertions;
using NerisSharp.Utils;
using NerisSharp.Models.ElementModels.Incident;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerisSharpTest.Utils
{
    public class EnumUtilTests
    {
        [Fact]
        public void EnumUtil_ValidList_ReturnsCorrectStrings()
        {
            List<IncidentTypeEnum> eList = new List<IncidentTypeEnum>() { IncidentTypeEnum.FIRE, IncidentTypeEnum.FIRE_ALARM, IncidentTypeEnum.WATER };
            List<string> result = EnumUtils.ReturnStringList(eList);
            result.Should().Equal("FIRE", "FIRE_ALARM", "WATER");
        }

        [Fact]
        public void EnumUtil_SubmitNull_ReturnsNull()
        {
            List<string> result = EnumUtils.ReturnStringList<IncidentTypeEnum>(null);
            result.Should().BeNull();
        }

        [Fact]
        public void Enumutil_SubmitNonEnum_Throws()
        {
            List<int> toSubmit = new List<int> { 1, 2, 3 };
            Func<List<string>> act = () =>
            {
                return EnumUtils.ReturnStringList<int>(toSubmit);
            };
            act.Should().ThrowExactly<ArgumentException>();
        }
    }
}
