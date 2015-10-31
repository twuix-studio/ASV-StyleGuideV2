using System;
using Fei.Applications.SAL;

namespace Fei.SliceAndView.Common.Utilities
{
    /// <summary>
    /// DetectorIdentificationHelper provides auxiliary methods that help user to identify detector types and modes.
    /// </summary>
    public static class DetectorIdentificationHelper
    {
        public enum DetectorTypeNameForm
        {
            Normal,
            Short
        }

        #region Extension methods related to detector type

        /// <summary>
        /// Converts <paramref name="type"/> to name.
        /// </summary>
        /// <param name="type">Detector type.</param>
        public static string ToName(this DetectorType type)
        {
            string detectorTypeName;

            switch (type)
            {
                case DetectorType.TLD:
                    detectorTypeName = "TLD";
                    break;
                case DetectorType.ETD:
                    detectorTypeName = "ETD";
                    break;
                case DetectorType.TLD2:
                    detectorTypeName = "TLD";
                    break;
                case DetectorType.BSD:
                    detectorTypeName = "BSED";
                    break;
                case DetectorType.DualBSD:
                    detectorTypeName = "Dual BSD";
                    break;
                case DetectorType.QuadBSD:
                    detectorTypeName = "Quad BSED";
                    break;
                case DetectorType.SingleBSD:
                    detectorTypeName = "vCD";
                    break;
                case DetectorType.GAD:
                case DetectorType.GAD2:
                    detectorTypeName = "Dual GAD";
                    break;
                case DetectorType.STEM:
                    detectorTypeName = "STEM I";
                    break;
                case DetectorType.STEM3:
                    detectorTypeName = "STEM 3";
                    break;
                case DetectorType.STEM3_GMode:
                    detectorTypeName = "STEM 3+";
                    break;
                case DetectorType.InColumnBSD:
                    detectorTypeName = "ICD";
                    break;
                case DetectorType.LFD:
                    detectorTypeName = "LFD";
                    break;
                case DetectorType.LVD:
                    detectorTypeName = "LVD";
                    break;
                case DetectorType.LVSED:
                    detectorTypeName = "LVSED";
                    break;
                case DetectorType.CRD:
                    detectorTypeName = "Helix";
                    break;
                case DetectorType.GSED:
                    detectorTypeName = "GSED";
                    break;
                case DetectorType.GBSD:
                    detectorTypeName = "GBSD";
                    break;
                case DetectorType.CDEM:
                    detectorTypeName = "CDEM";
                    break;
                case DetectorType.PMT:
                    detectorTypeName = "PMD - BSE";
                    break;
                case DetectorType.ICE:
                    detectorTypeName = "ICE";
                    break;
                case DetectorType.ABS:
                    detectorTypeName = "ABS";
                    break;
                case DetectorType.CBS:
                    detectorTypeName = "CBS";
                    break;
                case DetectorType.Mirror:
                    detectorTypeName = "MD";
                    break;
                case DetectorType.External:
                    detectorTypeName = "External";
                    break;
                case DetectorType.IR:
                    detectorTypeName = "CCD";
                    break;
                case DetectorType.IR2:
                    detectorTypeName = "CCD II";
                    break;
                case DetectorType.IRCameraDetector:
                    detectorTypeName = "IR Camera";
                    break;
                case DetectorType.OM:
                    detectorTypeName = "OM";
                    break;
                case DetectorType.NOC:
                    detectorTypeName = "Nav-Cam";
                    break;
                case DetectorType.HiResOptical:
                    detectorTypeName = "HiRes Optical";
                    break;
                case DetectorType.HiResOpticalLowMag:
                    detectorTypeName = "HiRes Optical LowMag";
                    break;
                case DetectorType.Mix:
                    detectorTypeName = "Mix";
                    break;
                case DetectorType.SICD:
                    detectorTypeName = "ICD";
                    break;
                case DetectorType.ECD:
                    detectorTypeName = "ECD";
                    break;
                case DetectorType.T1:
                    detectorTypeName = "T1";
                    break;
                case DetectorType.T2:
                    detectorTypeName = "T2";
                    break;
                case DetectorType.T3:
                    detectorTypeName = "T3";
                    break;
                case DetectorType.LmABS:
                    detectorTypeName = "GAD-ABS";
                    break;
                case DetectorType.LmCBS:
                    detectorTypeName = "GAD-CBS";
                    break;
                case DetectorType.VsABS:
                    detectorTypeName = "LV-ABS";
                    break;
                case DetectorType.VsCBS:
                    detectorTypeName = "LV-CBS";
                    break;
                case DetectorType.None:
                    detectorTypeName = "No Detector";
                    break;
                default:
                    detectorTypeName = ProvideFallbackName(type);
                    break;
            }

            return detectorTypeName;
        }

        /// <summary>
        /// Generates fallback name for the given detector type based on its string representation.
        /// The method was introduced in order to allow developers to distinguish new detectors
        /// until they are assigned a proper name.
        /// </summary>
        private static string ProvideFallbackName(DetectorType type)
        {
            string fallbackName = type.ToString();

            if (fallbackName.StartsWith("en") && fallbackName.Length > 2)
            {
                fallbackName = fallbackName.Substring(2);
            }

            return fallbackName;
        }

        public static string ToShortName(this DetectorType type)
        {
            string detectorTypeName;

            switch (type)
            {
                case DetectorType.QuadBSD:
                    detectorTypeName = "Quad BSED";
                    break;
                case DetectorType.PMT:
                    detectorTypeName = "PMD-BSE";
                    break;
                case DetectorType.HiResOptical:
                    detectorTypeName = "HRO";
                    break;
                case DetectorType.HiResOpticalLowMag:
                    detectorTypeName = "HRO-LM";
                    break;
                case DetectorType.None:
                    detectorTypeName = "No Det.";
                    break;
                default:
                    detectorTypeName = type.ToName();
                    break;
            }
            return detectorTypeName;
        }

        public static string ToName(this DetectorType type, DetectorTypeNameForm name)
        {
            switch (name)
            {
                case DetectorTypeNameForm.Normal:
                    return type.ToName();
                case DetectorTypeNameForm.Short:
                    return type.ToShortName();
                default:
                    throw new ArgumentException("Unknown name kind");
            }
        }

        #endregion

        #region Extension methods related to detector mode

        public static string ToLongName(this DetectorMode detectorMode, DetectorType detectorType)
        {
            switch (detectorType)
            {
                case DetectorType.STEM3_GMode:
                    switch (detectorMode)
                    {
                        case DetectorMode.Custom:
                            return "Custom Annular 1";
                        case DetectorMode.Custom2:
                            return "Custom Annular 2";
                        case DetectorMode.Custom3:
                            return "Custom Angular";
                        case DetectorMode.Custom4:
                            return "Custom Annular 3";
                        case DetectorMode.Custom5:
                            return "Custom Annular 4";
                        default:
                            return GetDefaultLongName(detectorMode);
                    }
                case DetectorType.CBS:
                    switch (detectorMode)
                    {
                        case DetectorMode.Custom:
                            return "Custom 1";
                        case DetectorMode.Custom2:
                            return "Custom 2";
                        default:
                            return GetDefaultLongName(detectorMode);
                    }
                default:
                    return GetDefaultLongName(detectorMode);
            }
        }

        private static string GetDefaultLongName(DetectorMode detectorMode)
        {
            switch (detectorMode)
            {
                case DetectorMode.SecondaryElectrons:
                    return "Secondary Electrons";
                case DetectorMode.BackscatterElectrons:
                    return "Backscatter Electrons";
                case DetectorMode.Custom:
                    return "Custom";
                case DetectorMode.Custom2:
                    return "Custom 2";
                case DetectorMode.Custom3:
                    return "Custom 3";
                case DetectorMode.Custom4:
                    return "Custom 4";
                case DetectorMode.Custom5:
                    return "Custom 5";
                case DetectorMode.DownHoleVisibility:
                    return "Down-hole visibility";
                case DetectorMode.ChargeNeutralization:
                    return "Charge Neutralization";
                case DetectorMode.SecondaryIons:
                    return "Secondary Ions";
                case DetectorMode.SegmentA:
                    return "Segment A";
                case DetectorMode.SegmentB:
                    return "Segment B";
                case DetectorMode.ZContrast:
                    return "Z Contrast";
                case DetectorMode.Topography:
                    return "Topography";
                case DetectorMode.AplusB:
                    return "A + B";
                case DetectorMode.AminusB:
                    return "A - B";
                case DetectorMode.BrightField:
                    return "Bright Field";
                case DetectorMode.DarkField:
                    return "Dark Field";
                case DetectorMode.DarkField1:
                    return "Dark Field 1";
                case DetectorMode.DarkField2:
                    return "Dark Field 2";
                case DetectorMode.DarkField3:
                    return "Dark Field 3";
                case DetectorMode.DarkField4:
                    return "Dark Field 4";
                case DetectorMode.Angular:
                    return "HAADF";
                case DetectorMode.AngularPartial:
                    return "HADF Partial";
                case DetectorMode.AngularPartialComplement:
                    return "HADF Partial Complement";
                case DetectorMode.Mix:
                    return "Mix";
                case DetectorMode.BeamDeceleration:
                    return "Deceleration Mode";
                case DetectorMode.LowAngle:
                    return "Inner";
                case DetectorMode.HighAngle:
                    return "Outer";
                case DetectorMode.AnularA:
                    return "A";
                case DetectorMode.AnularB:
                    return "B";
                case DetectorMode.AnularC:
                    return "C";
                case DetectorMode.AnularD:
                    return "D";
                case DetectorMode.All:
                    return "All";
                case DetectorMode.Scintillation:
                    return "Scintillation";
                default:
                    return "$ERROR$";
            }
        }

        public static string ToShortName(this DetectorMode detectorMode, DetectorType detectorType)
        {
            switch (detectorType)
            {
                case DetectorType.STEM3_GMode:
                    switch (detectorMode)
                    {
                        case DetectorMode.Custom:
                            return "Custom 1";
                        case DetectorMode.Custom2:
                            return "Custom 2";
                        case DetectorMode.Custom3:
                            return "Custom";
                        case DetectorMode.Custom4:
                            return "Custom 3";
                        case DetectorMode.Custom5:
                            return "Custom 4";
                        default:
                            return GetDefaultShortName(detectorMode);
                    }
                case DetectorType.CBS:
                    switch (detectorMode)
                    {
                        case DetectorMode.Custom:
                            return "Custom 1";
                        case DetectorMode.Custom2:
                            return "Custom 2";
                        default:
                            return GetDefaultLongName(detectorMode);
                    }
                default:
                    return GetDefaultShortName(detectorMode);
            }
        }

        private static string GetDefaultShortName(this DetectorMode detectorMode)
        {
            string shortName;
            switch (detectorMode)
            {
                case DetectorMode.SecondaryElectrons:
                    shortName = "SE";
                    break;

                case DetectorMode.BackscatterElectrons:
                    shortName = "BSE";
                    break;

                case DetectorMode.Custom:
                    shortName = "Custom";
                    break;

                case DetectorMode.Custom2:
                    shortName = "Custom";
                    break;

                case DetectorMode.Custom3:
                    shortName = "Custom";
                    break;

                case DetectorMode.Custom4:
                    shortName = "Custom";
                    break;

                case DetectorMode.Custom5:
                    shortName = "Custom";
                    break;

                case DetectorMode.DownHoleVisibility:
                    shortName = "DHV";
                    break;

                case DetectorMode.ChargeNeutralization:
                    shortName = "CN";
                    break;

                case DetectorMode.SecondaryIons:
                    shortName = "SI";
                    break;

                case DetectorMode.SegmentA:
                    shortName = "A";
                    break;

                case DetectorMode.SegmentB:
                    shortName = "B";
                    break;

                case DetectorMode.ZContrast:
                    shortName = "Z Contrast";
                    break;

                case DetectorMode.Topography:
                    shortName = "Topography";
                    break;

                case DetectorMode.AplusB:
                    shortName = "A+B";
                    break;

                case DetectorMode.AminusB:
                    shortName = "A-B";
                    break;

                case DetectorMode.BrightField:
                    shortName = "Bright Field";
                    break;

                case DetectorMode.DarkField:
                    shortName = "Dark Field";
                    break;

                case DetectorMode.DarkField1:
                    shortName = "Dark Field 1";
                    break;

                case DetectorMode.DarkField2:
                    shortName = "Dark Field 2";
                    break;

                case DetectorMode.DarkField3:
                    shortName = "Dark Field 3";
                    break;

                case DetectorMode.DarkField4:
                    shortName = "Dark Field 4";
                    break;

                case DetectorMode.Angular:
                    shortName = "HAADF";
                    break;

                case DetectorMode.AngularPartial:
                    shortName = "HADF-P";
                    break;

                case DetectorMode.AngularPartialComplement:
                    shortName = "HADF-PC";
                    break;

                case DetectorMode.Mix:
                    shortName = "Mix";
                    break;

                case DetectorMode.BeamDeceleration:
                    shortName = "Dec. Mode";
                    break;

                case DetectorMode.LowAngle:
                    shortName = "Inner";
                    break;

                case DetectorMode.HighAngle:
                    shortName = "Outer";
                    break;

                case DetectorMode.AnularA:
                    shortName = "A";
                    break;

                case DetectorMode.AnularB:
                    shortName = "B";
                    break;

                case DetectorMode.AnularC:
                    shortName = "C";
                    break;

                case DetectorMode.AnularD:
                    shortName = "D";
                    break;

                case DetectorMode.All:
                    shortName = "All";
                    break;

                case DetectorMode.Scintillation:
                    shortName = "Scint";
                    break;
                case DetectorMode.None:
                    shortName = "None";
                    break;
                default:
                    shortName = "$ERROR$";
                    break;
            }
            return shortName;
        }

        #endregion
    }
}
