using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFilterDuctPipeWithSQL.Model
{
    public static class CreateDataPipe
    {
        public static void Create(SqlConnection conec, ObservableCollection<Pipe> Pipes) 
        {
            //SqlConnection conec = new SqlConnection("Data Source=DESKTOP-MTQUN8L\\BUHII;Initial Catalog=MEP;Integrated Security=True");
            conec.Open();
            using (SqlCommand command2 = new SqlCommand("DBCC CHECKIDENT ('MEP_Pipe', RESEED, 0)", conec))
            {
                command2.ExecuteNonQuery();
            }
            using (SqlCommand command1 = new SqlCommand("DELETE FROM MEP_Pipe", conec))
            {         
                command1.ExecuteNonQuery();                 
            }

            var commnand = new SqlCommand();
            commnand.Connection = conec;
            commnand.CommandText = "INSERT INTO MEP_Pipe (ElementID,NameTypePipe,Length, Size, SystemClass,Comments) VALUES (@ElementID,@NameTypePipe,@Length,@Size,@SystemClass,@Comments)";

            var ElementID = commnand.Parameters.AddWithValue("@ElementID", "");
            var NameTypePipe = commnand.Parameters.AddWithValue("@NameTypePipe", "");
            var Length = commnand.Parameters.AddWithValue("@Length", "");
            var Size = commnand.Parameters.AddWithValue("@Size", "");
            var SystemClass = commnand.Parameters.AddWithValue("@SystemClass", "");
            var Comments = commnand.Parameters.AddWithValue("@Comments", "");



            foreach (Pipe p in Pipes)
            {
                ElementID.Value = p.Id.ToString();
                NameTypePipe.Value = p.Name;
                Length.Value = (p.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble()) * 304.8;
                Size.Value = p.get_Parameter(BuiltInParameter.RBS_CALCULATED_SIZE).AsValueString();
                SystemClass.Value = p.get_Parameter(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM).AsValueString().ToString();
                Comments.Value = p.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsValueString() != null ? p.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsValueString() : String.Empty;

                commnand.ExecuteNonQuery();
            }
            conec.Close();
        }
    }
}
