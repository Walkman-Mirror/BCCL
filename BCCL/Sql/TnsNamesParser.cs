// Author: ciro_vladimir
// TNSNames Reader
// http://www.codeproject.com/KB/database/TNSNames_Reader.aspx
// License: The Code Project Open License (CPOL)
// http://www.codeproject.com/info/cpol10.aspx


using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace BCCL.Sql
{
    public static class TnsNamesParser
    {
        public static List<string> GetOracleHomes()
        {
            var oracleHomes = new List<string>();
            RegistryKey rgkLM = Registry.LocalMachine;
            RegistryKey rgkAllHome = rgkLM.OpenSubKey(@"SOFTWARE\ORACLE");
            if (rgkAllHome != null)
            {
                foreach (string subkey in rgkAllHome.GetSubKeyNames())
                {
                    if (subkey.StartsWith("KEY_"))
                        oracleHomes.Add(subkey);
                }
            }
            return oracleHomes;
        }

        private static string GetOracleHomePath(String oracleHomeRegistryKey)
        {
            RegistryKey rgkLM = Registry.LocalMachine;
            RegistryKey rgkOracleHome = rgkLM.OpenSubKey(@"SOFTWARE\ORACLE\" +
                oracleHomeRegistryKey);

            if (rgkOracleHome.Equals(""))
                return rgkOracleHome.GetValue("ORACLE_HOME").ToString();
            return "";
        }

        private static string GetTnsNamesOraFilePath(String oracleHomeRegistryKey)
        {
            string oracleHomePath = GetOracleHomePath(oracleHomeRegistryKey);
            string tnsNamesOraFilePath = "";
            if (!oracleHomePath.Equals(""))
            {
                tnsNamesOraFilePath = oracleHomePath + @"\NETWORK\ADMIN\TNSNAMES.ORA";
                if (!(System.IO.File.Exists(tnsNamesOraFilePath)))
                {
                    tnsNamesOraFilePath = oracleHomePath + @"\NET80\ADMIN\TNSNAMES.ORA";
                }
            }
            return tnsNamesOraFilePath;
        }

        public static List<string> LoadTnsNames(string oracleHomeRegistryKey)
        {
            string strTnsNamesOraFilePath = GetTnsNamesOraFilePath(oracleHomeRegistryKey);

            return LoadTnsNamesFile(strTnsNamesOraFilePath);
        }

        private static List<string> LoadTnsNamesFile(string tnsNamesOraFile)
        {
            var namesCollection = new List<string>();
            string regExPattern = @"[\n][\s]*[^\(][a-zA-Z0-9_.]+[\s]*=[\s]*\(";
            if (!tnsNamesOraFile.Equals(""))
            {
                //check out that file does physically exists
                var fiTns = new System.IO.FileInfo(tnsNamesOraFile);
                if (fiTns.Exists)
                {
                    if (fiTns.Length > 0)
                    {
                        //read tnsnames.ora file
                        int iCount;
                        for (iCount = 0; iCount < Regex.Matches(
                            System.IO.File.ReadAllText(fiTns.FullName),
                            regExPattern).Count; iCount++)
                        {
                            namesCollection.Add(Regex.Matches(
                                System.IO.File.ReadAllText(fiTns.FullName),
                                regExPattern)[iCount].Value.Trim().Substring(0,
                                                                    Regex.Matches(System.IO.File.ReadAllText(fiTns.FullName),
                                                                                regExPattern)[iCount].Value.Trim().IndexOf(" ")));
                        }
                    }
                }
            }
            return namesCollection;
        }
    }
}