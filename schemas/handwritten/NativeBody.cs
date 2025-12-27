// ---------------------------------------------------------------------------
// <summary>
//     Hand-maintained NativeBody class for CargoWise Native schema.
//     Add new entity types here as needed.
// </summary>
// <remarks>
// This class is hand-maintained due to limitations in the code generation tool, and WiseTech's schema design.
// </remarks>
// ---------------------------------------------------------------------------

namespace CargoWiseNetLibrary.Models.Native
{
    using System;
    using System.CodeDom.Compiler;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the Body element of a Native message.
    /// Contains a single entity element (Product, Organization, etc.).
    /// </summary>
    [GeneratedCode("HandWritten", "1.0.0")]
    [Serializable]
    [XmlType(
        "NativeBody",
        Namespace = "http://www.cargowise.com/Schemas/Native/2011/11",
        AnonymousType = true
    )]
    [XmlRoot("NativeBody", Namespace = "http://www.cargowise.com/Schemas/Native/2011/11")]
    public partial class NativeBody
    {
        /// <summary>
        /// Gets or sets the AcceptabilityBand data.
        /// </summary>
        [XmlElement("AcceptabilityBand")]
        public AcceptabilityBandData AcceptabilityBand { get; set; }

        /// <summary>
        /// Gets or sets the Airline data.
        /// </summary>
        [XmlElement("Airline")]
        public AirlineData Airline { get; set; }

        /// <summary>
        /// Gets or sets the BMSystem data.
        /// </summary>
        [XmlElement("BMSystem")]
        public BMSystemData BMSystem { get; set; }

        /// <summary>
        /// Gets or sets the CommodityCode data.
        /// </summary>
        [XmlElement("CommodityCode")]
        public CommodityCodeData CommodityCode { get; set; }

        /// <summary>
        /// Gets or sets the Company data.
        /// </summary>
        [XmlElement("Company")]
        public CompanyData Company { get; set; }

        /// <summary>
        /// Gets or sets the Container data.
        /// </summary>
        [XmlElement("Container")]
        public ContainerData Container { get; set; }

        /// <summary>
        /// Gets or sets the Country data.
        /// </summary>
        [XmlElement("Country")]
        public CountryData Country { get; set; }

        /// <summary>
        /// Gets or sets the CurrencyExchangeRate data.
        /// </summary>
        [XmlElement("CurrencyExchangeRate")]
        public CurrencyExchangeRateData CurrencyExchangeRate { get; set; }

        /// <summary>
        /// Gets or sets the CusStatement data.
        /// </summary>
        [XmlElement("CusStatement")]
        public CusStatementData CusStatement { get; set; }

        /// <summary>
        /// Gets or sets the DangerousGood data.
        /// </summary>
        [XmlElement("DangerousGood")]
        public DangerousGoodData DangerousGood { get; set; }

        /// <summary>
        /// Gets or sets the Organization data.
        /// </summary>
        [XmlElement("Organization")]
        public OrganizationData Organization { get; set; }

        /// <summary>
        /// Gets or sets the Product data.
        /// </summary>
        [XmlElement("Product")]
        public ProductData Product { get; set; }

        /// <summary>
        /// Gets or sets the Rate data.
        /// </summary>
        [XmlElement("Rate")]
        public RateData Rate { get; set; }

        /// <summary>
        /// Gets or sets the ServiceLevel data.
        /// </summary>
        [XmlElement("ServiceLevel")]
        public ServiceLevelData ServiceLevel { get; set; }

        /// <summary>
        /// Gets or sets the Staff data.
        /// </summary>
        [XmlElement("Staff")]
        public StaffData Staff { get; set; }

        /// <summary>
        /// Gets or sets the Tag data.
        /// </summary>
        [XmlElement("Tag")]
        public TagData Tag { get; set; }

        /// <summary>
        /// Gets or sets the TagRule data.
        /// </summary>
        [XmlElement("TagRule")]
        public TagRuleData TagRule { get; set; }

        /// <summary>
        /// Gets or sets the UNLOCO data.
        /// </summary>
        [XmlElement("UNLOCO")]
        public UNLOCOData UNLOCO { get; set; }

        /// <summary>
        /// Gets or sets the Vessel data.
        /// </summary>
        [XmlElement("Vessel")]
        public VesselData Vessel { get; set; }

        /// <summary>
        /// Gets or sets the WorkflowTemplate data.
        /// </summary>
        [XmlElement("WorkflowTemplate")]
        public WorkflowTemplateData WorkflowTemplate { get; set; }
    }
}
