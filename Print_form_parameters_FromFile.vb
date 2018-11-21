#If DEBUG Then        ' If the file exist read file to print the custom form else do nothing 
                Dim report_name_from_file As String()
                If System.IO.File.Exists("C:\Form_Test\form_to_print.txt") Then
                    report_name_from_file = System.IO.File.ReadAllLines("C:\Form_Test\form_to_print.txt")
                    reportName = report_name_from_file(0)
                    If report_name_from_file.Length > 1 Then
                        For Each item As String In report_name_from_file.Skip(1)
                            Dim variable_cambiar As String() = item.Split("|")
                            If variable_cambiar.Count > 1 Then
                                Try
                                    Dim dynClass As Object
                                    Dim name As String = variable_cambiar(1)
                                    Dim var_type As Type = Nothing
                                    If name = "System.String" Then
                                        ds.Tables(2)(0)(variable_cambiar(0)) = variable_cambiar(2)
                                        Continue For
                                    End If
                                    'base = Reflection.Assembly.GetEntryAssembly.GetType(name, False, True)
                                    If var_type Is Nothing Then
                                        var_type = Reflection.Assembly.GetExecutingAssembly.GetType(name, False, True)
                                        If var_type Is Nothing Then
                                            For Each assembly As Reflection.Assembly In
                                            AppDomain.CurrentDomain.GetAssemblies
                                                var_type = assembly.GetType(name, False, True)
                                                If var_type IsNot Nothing Then
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End If
                                    dynClass = System.Activator.CreateInstance(var_type)
                                    dynClass = Convert.ChangeType(variable_cambiar(2), var_type)
                                    Console.Write(dynClass.ToString())
                                    ds.Tables(2)(0)(variable_cambiar(0)) = dynClass

                                Catch ex As Exception
                                    Console.Write("boom!!!")
                                End Try

                            End If
                        Next
                    End If
                End If
#End If



#If DEBUG Then
                    If System.IO.Directory.Exists("C:\Form_Test\") Then
                        Dim fts As String = Date.Now.Ticks
                        Dim filenametopdf As String = "C:\Form_Test\" & reportName.Remove(0, 1).Remove(reportName.Length - 6, 5) & "-" & fts & ".pdf"
                        System.IO.File.WriteAllBytes(filenametopdf, bytes)
                        Diagnostics.Process.Start(filenametopdf)
                    End If

#End If
