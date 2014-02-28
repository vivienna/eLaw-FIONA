Imports Microsoft.VisualBasic

''' 
''' <filename>
''' clsConfigs.vb
''' </filename>
''' <description> 
''' This purpose of this file is to provide global accessibility of some variable may be for development purpose
''' </description>
''' <author>
''' usman sarwar
''' </author>
''' <date>
''' 08 april 2011
''' </date>
''' 
''' <todo>
''' 1- need to read from config.web file the configurations.
''' 2- need to check what are the new configurations useful
''' </todo>
''' 


Public Class clsConfigs

    'Public Shared ReadOnly sGlobalConnectionString As String = "Data Source=localhost;initial catalog=elawdb_new;uid=sa;pwd=admin"
    '//remote server
    'Public Shared ReadOnly sGlobalConnectionString As String = "Data Source=SERVER6161\SQL2012;initial catalog=elawdb;uid=mldadmin;pwd=2ZygAEq!"
    Public Shared ReadOnly sGlobalConnectionString As String = "Data Source=DATASOLUTIONS;initial catalog=elawdb;uid=sa;pwd=D4t4S0lut10n$sqL"
    
    'Public Shared ReadOnly sGlobalConnectionString As String = "Data Source=ME-PC;initial catalog=elawdb;uid=sa;pwd=admin@2012"
    'Public Shared ReadOnly sGlobalConnectionString As String = "Data Source=111.90.137.221;initial catalog=elawdb;uid=mldadmin;pwd=2ZygAEq!"

End Class
