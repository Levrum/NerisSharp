using FluentAssertions;
using NerisLibrary.Models.ElementModels.Incident;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NerisLibraryTest.Models
{
    public class IncidentTypeTests
    {
        //TO test:
        // Type string created with 3 succesful subtypes
        // Type string fails with bad types
        // invalid type is blank string and invalid
        // type with one type has no ||

        [Fact]
        public void IncidentType_ValidTypes_CreatesCorrectTypeStringAndList()
        {
            IncidentType incidentType = new IncidentType(IncidentTypeEnum.FIRE, IncidentTypeEnum.STRUCTURE_FIRE, IncidentTypeEnum.STRUCTURAL_INVOLVEMENT_FIRE);
            incidentType.Valid.Should().BeTrue();
            incidentType.Type.Should().Be("FIRE||STRUCTURE_FIRE||STRUCTURAL_INVOLVEMENT_FIRE");
            incidentType.TypeList.ToArray().Should().BeEquivalentTo([IncidentTypeEnum.FIRE, IncidentTypeEnum.STRUCTURE_FIRE, IncidentTypeEnum.STRUCTURAL_INVOLVEMENT_FIRE]);
        }
        [Fact]
        public void IncidentType_ValidSingleType_CreatesCorrectString()
        {
            IncidentType incidentType = new IncidentType(IncidentTypeEnum.FIRE);
            incidentType.Type.Should().NotContain("||");
        }

        [Theory]
        [InlineData(IncidentTypeEnum.STRUCTURE_FIRE, null, null)]
        [InlineData(IncidentTypeEnum.FIRE, IncidentTypeEnum.STRUCTURAL_INVOLVEMENT_FIRE, null)]
        [InlineData(IncidentTypeEnum.FIRE, IncidentTypeEnum.STRUCTURE_FIRE, IncidentTypeEnum.FIRE)]
        public void IncidentType_InvalidTypeLevel_CreatesInvalidType(IncidentTypeEnum type1, IncidentTypeEnum? type2, IncidentTypeEnum? type3)
        {
            IncidentType incidentType = new IncidentType(type1, type2, type3);
            incidentType.Valid.Should().BeFalse();
        }

        [Fact]
        public void IncidentType_InvalidType_HasBlankString()
        {
            IncidentType incidentType = new IncidentType(IncidentTypeEnum.STRUCTURE_FIRE, IncidentTypeEnum.STRUCTURAL_INVOLVEMENT_FIRE);
            incidentType.Valid.Should().BeFalse();
            incidentType.Type.Should().BeNullOrEmpty();
            incidentType.TypeList.Should().BeEmpty();
        }

        [Fact]
        public void IncidentType_Type3WithNoType2_ThrowsArgumentNullException()
        {
            Func<IncidentType> act = () => { 
                IncidentType incidentType = new IncidentType(IncidentTypeEnum.FIRE, null, IncidentTypeEnum.STRUCTURAL_INVOLVEMENT_FIRE);
                return incidentType;
            };
            act.Should().ThrowExactly<ArgumentException>();
        }



    }
}
