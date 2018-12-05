 using System;
using System.Globalization;
using System.IO;
using Core.Collections.Generic;
using Core.Reflection;
using Core.Serialization;

namespace Hdc.Mv.Inspection
{
    public static class InspectionSchemaExtensions
    {
        public static InspectionSchema LoadFromAssemblyDir(this string shortFileName)
        {
            var dir = typeof(InspectionSchemaExtensions).Assembly.GetAssemblyDirectoryPath();
            var fileName = Path.Combine(dir, shortFileName);
            if (!File.Exists(fileName))
            {
                var ds = InspectionSchemaFactory.CreateDefaultSchema();
                ds.SerializeToXamlFile(fileName);
            }
            var schema = fileName.DeserializeFromXamlFile<InspectionSchema>();
            return schema;
        }

        public static InspectionSchema LoadFromFile(this string fileName)
        {
            if (!File.Exists(fileName))
            {
                var ds = InspectionSchemaFactory.CreateDefaultSchema();
                ds.SerializeToXamlFile(fileName);
            }
            var schema = fileName.DeserializeFromXamlFile<InspectionSchema>();
            return schema;
        }

        public static void Merge(this InspectionSchema masterInspectionSchema, InspectionSchema slaveInspectionSchema)
        {
            masterInspectionSchema.CoordinateCircles.AddRange(slaveInspectionSchema.CoordinateCircles);
            masterInspectionSchema.CoordinateEdges.AddRange(slaveInspectionSchema.CoordinateEdges);
            masterInspectionSchema.CoordinateRegions.AddRange(slaveInspectionSchema.CoordinateRegions);
            masterInspectionSchema.CoordinateExtactors.AddRange(slaveInspectionSchema.CoordinateExtactors);

            if(masterInspectionSchema.DatumPlaneCreator == null)
                masterInspectionSchema.DatumPlaneCreator = slaveInspectionSchema.DatumPlaneCreator;

            masterInspectionSchema.EdgeSearchingDefinitions.AddRange(slaveInspectionSchema.EdgeSearchingDefinitions);
            masterInspectionSchema.PartSearchingDefinitions.AddRange(slaveInspectionSchema.PartSearchingDefinitions);
            masterInspectionSchema.CircleSearchingDefinitions.AddRange(slaveInspectionSchema.CircleSearchingDefinitions);
            masterInspectionSchema.DistanceBetweenLinesDefinitions.AddRange(slaveInspectionSchema.DistanceBetweenLinesDefinitions);
            masterInspectionSchema.DistanceBetweenIntersectionPointsDefinitions.AddRange(slaveInspectionSchema.DistanceBetweenIntersectionPointsDefinitions);
            masterInspectionSchema.SurfaceDefinitions.AddRange(slaveInspectionSchema.SurfaceDefinitions);
            masterInspectionSchema.DefectDefinitions.AddRange(slaveInspectionSchema.DefectDefinitions);
#pragma warning disable 618
            masterInspectionSchema.RegionTargetDefinitions.AddRange(slaveInspectionSchema.RegionTargetDefinitions);
#pragma warning restore 618
            masterInspectionSchema.RegionSearchingDefinitions.AddRange(slaveInspectionSchema.RegionSearchingDefinitions);
            masterInspectionSchema.DataCodeSearchingDefinitions.AddRange(slaveInspectionSchema.DataCodeSearchingDefinitions);
            masterInspectionSchema.XldSearchingDefinitions.AddRange(slaveInspectionSchema.XldSearchingDefinitions);
            masterInspectionSchema.ReferencePointDefinitions.AddRange(slaveInspectionSchema.ReferencePointDefinitions);
            masterInspectionSchema.ReferenceLineDefinitions.AddRange(slaveInspectionSchema.ReferenceLineDefinitions);
            masterInspectionSchema.ReferenceRadialLineDefinitions.AddRange(slaveInspectionSchema.ReferenceRadialLineDefinitions);
            masterInspectionSchema.PointOfXldAndRadialLineDefinitions.AddRange(slaveInspectionSchema.PointOfXldAndRadialLineDefinitions);
            masterInspectionSchema.PointOfEdgeAndRadialLineDefinitions.AddRange(slaveInspectionSchema.PointOfEdgeAndRadialLineDefinitions);
            masterInspectionSchema.DistanceBetweenPointsOfXldAndRadialLineDefinitions.AddRange(slaveInspectionSchema.DistanceBetweenPointsOfXldAndRadialLineDefinitions);
            masterInspectionSchema.DistanceBetweenTwoPointsDefinitions.AddRange(slaveInspectionSchema.DistanceBetweenTwoPointsDefinitions);
            masterInspectionSchema.StepFromGrayValueDefinitions.AddRange(slaveInspectionSchema.StepFromGrayValueDefinitions);
        }


        public static InspectionSchema GetInspectionSchemaFromDir(this string dir)
        {
            var inspectionSchemaDirPath = dir;
            var inspectionSchemaFilePath = dir + "\\InspectionSchema.xaml";

            if (!Directory.Exists(inspectionSchemaDirPath))
            {
                Directory.CreateDirectory(inspectionSchemaDirPath);
            }

            if (!File.Exists(inspectionSchemaFilePath))
            {
                var ds = InspectionSchemaFactory.CreateDefaultSchema();
                ds.SerializeToXamlFile(inspectionSchemaFilePath);
            }


            InspectionSchema schema;
            try
            {
                schema = inspectionSchemaFilePath.DeserializeFromXamlFile<InspectionSchema>();
            }
            catch (Exception )
            {
                throw;
            }

            var files = Directory.GetFiles(inspectionSchemaDirPath);
            foreach (var file in files)
            {
                if (file == inspectionSchemaFilePath)
                    continue;

                if (!file.EndsWith(".xaml", true, CultureInfo.InvariantCulture))
                    continue;

                var slaveSchema = file.DeserializeFromXamlFile<InspectionSchema>();
                if (!slaveSchema.Disabled)
                    schema.Merge(slaveSchema);
            }

            return schema;
        }
    }
}