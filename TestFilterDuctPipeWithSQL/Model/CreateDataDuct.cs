using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFilterDuctPipeWithSQL.Model
{
    public static class CreateDataDuct
    {
        public static void Create(SqlConnection conec, ObservableCollection<Duct> Ducts)
        {
            conec.Open();
            using (SqlCommand command2 = new SqlCommand("DBCC CHECKIDENT ('MEP_Duct', RESEED, 0)", conec))
            {
                command2.ExecuteNonQuery();
            }
            using (SqlCommand command1 = new SqlCommand("DELETE FROM MEP_Duct", conec))
            {
                command1.ExecuteNonQuery();
            }

            var commnand = new SqlCommand();
            commnand.Connection = conec;
            commnand.CommandText = "INSERT INTO MEP_Duct (ElementID,NameTypeDuct,Length,Height,Width, SystemClass,Comments) VALUES (@ElementID,@NameTypeDuct,@Length,@Height,@Width,@SystemClass,@Comments)";

            var ElementID = commnand.Parameters.AddWithValue("@ElementID", "");
            var NameTypeDuct = commnand.Parameters.AddWithValue("@NameTypeDuct", "");
            var Length = commnand.Parameters.AddWithValue("@Length", "");

            var Height = commnand.Parameters.AddWithValue("@Height", "");
            var Width = commnand.Parameters.AddWithValue("@Width", "");

            var SystemClass = commnand.Parameters.AddWithValue("@SystemClass", "");
            var Comments = commnand.Parameters.AddWithValue("@Comments", "");
            foreach (Duct d in Ducts)
            {
                ElementID.Value = d.Id.ToString();
                NameTypeDuct.Value = d.Name;
                Length.Value = (d.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble()) * 304.8;

                Height.Value = (d.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM).AsDouble()) * 304.8;
                Width.Value = (d.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM).AsDouble()) * 304.8;

                SystemClass.Value = d.get_Parameter(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM).AsValueString().ToString();

                Comments.Value = d.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsValueString() != null ? d.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsValueString() : String.Empty;


                commnand.ExecuteNonQuery();
            }
            conec.Close();
        }
    }
}
