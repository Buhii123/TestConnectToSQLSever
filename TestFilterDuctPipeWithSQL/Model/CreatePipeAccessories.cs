using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFilterDuctPipeWithSQL.Model
{
    public static class CreatePipeAccessories
    {
        public static void Create(SqlConnection conec, ObservableCollection<FamilyInstance> PipesA)
        {
            //SqlConnection conec = new SqlConnection("Data Source=DESKTOP-MTQUN8L\\BUHII;Initial Catalog=MEP;Integrated Security=True");
            conec.Open();
            using (SqlCommand command2 = new SqlCommand("DBCC CHECKIDENT ('MEP_PipeAccessories', RESEED, 0)", conec))
            {
                command2.ExecuteNonQuery();
            }
            using (SqlCommand command1 = new SqlCommand("DELETE FROM MEP_PipeAccessories", conec))
            {
                command1.ExecuteNonQuery();
            }

            var commnand = new SqlCommand();
            commnand.Connection = conec;
            commnand.CommandText = "INSERT INTO MEP_PipeAccessories (ElementID,NameTypePipe, Size, SystemClass,Comments) VALUES (@ElementID,@NameTypePipe,@Size,@SystemClass,@Comments)";

            var ElementID = commnand.Parameters.AddWithValue("@ElementID", "");
            var NameTypePipe = commnand.Parameters.AddWithValue("@NameTypePipe", "");

            var Size = commnand.Parameters.AddWithValue("@Size", "");
            var SystemClass = commnand.Parameters.AddWithValue("@SystemClass", "");
            var Comments = commnand.Parameters.AddWithValue("@Comments", "");



            foreach (FamilyInstance p in PipesA)
            {
                try
                {
                    if (p != null)
                    {
                        ElementID.Value = p.Id.ToString();
                        NameTypePipe.Value = p.Name;
                        Size.Value = p.get_Parameter(BuiltInParameter.RBS_CALCULATED_SIZE).AsValueString();
                        SystemClass.Value = p.get_Parameter(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM).AsValueString().ToString();
                        Comments.Value = p.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsValueString() != null ? p.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsValueString() : String.Empty;

                        commnand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }



            }
            conec.Close();
        }

    }
}
