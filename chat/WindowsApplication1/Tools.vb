Imports System.Security.Cryptography
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Net
Imports System.IO

Module Tools
    Public ReadOnly Property getipv4address As IPAddress
        Get
            Return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(Function(a As IPAddress) Not a.IsIPv6LinkLocal AndAlso Not a.IsIPv6Multicast AndAlso Not a.IsIPv6SiteLocal).First()
        End Get
    End Property
    Sub encrypt(stream As System.IO.Stream, ByVal toencrypt As Object, ByVal key() As Byte)
        Dim riji As New System.Security.Cryptography.RijndaelManaged

        Dim cstream As New CryptoStream(stream, riji.CreateEncryptor(generate_key(key), generate_IV(key)), CryptoStreamMode.Write)
        Dim serializer As New Formatters.Binary.BinaryFormatter
        serializer.Serialize(cstream, toencrypt)

        cstream.Close()
    End Sub
    Function decrypt(stream As System.IO.Stream, ByVal key() As Byte) As Object

        Dim cryptStream As CryptoStream
        Dim riji As New System.Security.Cryptography.RijndaelManaged 'Declare CryptoServiceProvider.
        cryptStream = New CryptoStream(stream, riji.CreateDecryptor(generate_key(key), generate_IV(key)), CryptoStreamMode.Read)
        Dim serializer As New Formatters.Binary.BinaryFormatter
        Return serializer.Deserialize(cryptStream)
        cryptStream.Close()
        stream.Close()
    End Function
    Function getipfromclient(client As Sockets.TcpClient) As String
        Dim ip As String = ""
        Try
            ' Get the clients IP address using Client property            
            Dim ipend As Net.IPEndPoint = client.Client.RemoteEndPoint
            If Not ipend Is Nothing Then
                ip = ipend.Address.ToString
            End If
        Catch
            ip = ""
        End Try
        Return ip
    End Function
    Private Function generate_key(ByVal bytearray() As Byte) As Byte()

        Dim SHA512 As New System.Security.Cryptography.SHA512Managed

        Dim bytResult As Byte() = SHA512.ComputeHash(bytearray)
        Dim bytKey(31) As Byte

        For i As Integer = 0 To 31
            bytKey(i) = bytResult(i)
        Next
        Return bytKey
    End Function

    Private Function generate_IV(ByVal bytearray() As Byte) As Byte()

        Dim SHA512 As New System.Security.Cryptography.SHA512Managed
        Dim Result As Byte() = SHA512.ComputeHash(bytearray)

        Dim bytIV(15) As Byte

        For i As Integer = 32 To 47 'Get bytes for IV
            bytIV(i - 32) = Result(i)
        Next

        Return bytIV
    End Function

    Private Declare Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal process As IntPtr, ByVal minimumWorkingSetSize As Integer, ByVal maximumWorkingSetSize As Integer) As Integer
    Public Sub FlushMemory()
        Try
            GC.Collect()
            GC.WaitForPendingFinalizers()
            If Environment.OSVersion.Platform = PlatformID.Win32NT Then
                SetProcessWorkingSetSize(Process.GetCurrentProcess.Handle, -1, -1)
                Dim myProcesses As Process() = Process.GetProcessesByName("Applica­tionName")
                Dim myProcess As Process

                For Each myProcess In myProcesses
                    SetProcessWorkingSetSize(myProcess.Handle, -1, -1)
                Next

            End If
        Catch ex As Exception
        End Try
    End Sub
    ''' <summary>
    ''' Matches an Ip Address to the User from the Known Users List
    ''' </summary>
    ''' <param name="IpAddress">The Ip to match</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function MatchIpToUser(IpAddress As String) As User
        For i = 0 To KnownUsers.Count - 1
            If KnownUsers(i).IpAddress = IpAddress Then
                Return KnownUsers(i)
                Exit Function
            End If
        Next
        Return Nothing
    End Function
    ''' <summary>
    ''' Returns the Index of the specified Ip in the KnownUser list
    ''' </summary>
    ''' <param name="IpAddress"> The Ip to search for</param>
    ''' <returns>The Index of the Ip. Returns -1 if Ip Addres is not found</returns>
    ''' <remarks></remarks>
    Public Function MatchIpToUserInt(IpAddress As String) As Integer
        For i = 0 To KnownUsers.Count - 1
            If KnownUsers(i).IpAddress = IpAddress Then
                Return i
                Exit Function
            End If
        Next
        Return -1
    End Function
    Public Function MatchIpToUserKey(ByVal IpAddress As String) As Byte()
        For i = 0 To KnownUsers.Count - 1
            If KnownUsers(i).IpAddress = IpAddress Then
                Return KnownUsers(i).key
                Exit For
            End If
        Next
        Return Nothing
    End Function
End Module
