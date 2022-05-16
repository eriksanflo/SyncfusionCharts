using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Text;

namespace ElementosComunes.Conexiones
{
    public class TJSON
    {
        public static JArray JSONStringToJArray(string JSONString)
        {
            try
            {
                // necesaria esta validación por windows 10
                if (JSONString == "{}")
                    return new JArray();
                else
                {
                    string json = RespuestaToJsonArray(JSONString);
                    return JArray.Parse(json);
                }
            }
            catch (NullReferenceException e)
            {
                return new JArray();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /* Métodos privados */

        private static string RespuestaToJsonArray(string JSONString)
        {
            string respuesta = "";

            DataTable DataTableRespuesta = JSONStringToDataTable(JSONString);
            respuesta = DataTableToJSONStringArray(DataTableRespuesta);

            return respuesta;
        }

        private static DataTable JSONStringToDataTable(string JSONString)
        {
            JObject JObjectTemporal = JsonConvert.DeserializeObject<JObject>(JSONString); 

            DataTable DataTableRespuesta = new DataTable();

            if (JObjectTemporal.Count == 0)
                DataTableRespuesta = null;

            for (int i = 0; i < JObjectTemporal.Count; i++)
            {
                DataRow DataRowTabla = DataTableRespuesta.NewRow();
                if (i == 0)
                {
                    foreach (var par in JObjectTemporal["fila" + (i + 1)])
                    {
                        string columna = par[0].ToString();
                        string valor = par[1].ToString();

                        DataTableRespuesta.Columns.Add(columna);
                        DataRowTabla[columna] = valor;
                    }
                }
                else
                {
                    foreach (var par in JObjectTemporal["fila" + (i + 1)])
                    {
                        string columna = par[0].ToString();
                        string valor = par[1].ToString();

                        DataRowTabla[columna] = valor;
                    }
                }
                DataTableRespuesta.Rows.Add(DataRowTabla);
            }
            return DataTableRespuesta;
        }              

        private static string DataTableToJSONStringArray(DataTable DataTableRespuesta)
        {
            var JSONString = new StringBuilder();
            if (DataTableRespuesta.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < DataTableRespuesta.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < DataTableRespuesta.Columns.Count; j++)
                    {
                        if (j < DataTableRespuesta.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + DataTableRespuesta.Columns[j].ColumnName.ToString() + "\":" + "\"" + DataTableRespuesta.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == DataTableRespuesta.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + DataTableRespuesta.Columns[j].ColumnName.ToString() + "\":" + "\"" + DataTableRespuesta.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == DataTableRespuesta.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
    }
}
