Imports System.Net.Dns
Imports System.Net

Module Scanner
    Private ScanTimer As New Timer()
    ''' <summary>
    ''' When the progress gets updated, this event will occur
    ''' </summary>
    ''' <param name="Ips">All new IPHostEntries detected in the network Note: This can be 0</param>
    ''' <param name="progress">Progress in Percent</param>
    ''' <remarks></remarks>
    Public Event ProgressUpdated(ByVal IPs As List(Of String), ByVal progress As Single)
    Public Event FinishedScan()
    Sub ScanNetwork()
        Dim subnetmask(3) As Byte
        Dim defaultgateway(3) As Byte
        Dim SomeRandomNumber As Integer

        Dim oAdapter As Object = Nothing
        For Each oAdapter In GetObject("winmgmts:").execquery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True") 'get networkinformation
        Next
        Dim temparray() As String 'just to store both values, the string results(XXX.XXX.XXX.XXX YY) and split them up
        temparray = Split(Join(oAdapter.IPSubnet)) 'splits sub-net mask from random number
        subnetmask = SplitIp(temparray(0)) 'splits subnetmask into bytearray
        SomeRandomNumber = temparray(1) 'you never know what to do with that...

        If oAdapter.DefaultIPGateway IsNot Nothing Then
            If oAdapter.DefaultIPGateway Is DBNull.Value Then
                defaultgateway = SplitIp(getipv4address.ToString) 'in case the computer cant access default gateway
                defaultgateway(3) = 1
            Else
                defaultgateway = SplitIp(Join(oAdapter.DefaultIPGateway)) ' gets and splits defaultgateway if possible
            End If

        Else
            Exit Sub
        End If

        For k = defaultgateway(1) To 255 - subnetmask(1) + defaultgateway(1)
            For j = defaultgateway(2) To 255 - subnetmask(2) + defaultgateway(2)
                For i = defaultgateway(3) To 254 - subnetmask(3) + defaultgateway(3)
                    Dim ip As String = defaultgateway(0) & "." & k & "." & j & "." & i
                    If Not ip = defaultgateway(0) & "." & defaultgateway(1) & "." & defaultgateway(2) & "." & defaultgateway(3) Then
                        pingstorage.aimedsize += 1
                        Dim t As New Threading.Thread(Sub() sendping(ip)) 'start  sub async for ping
                        t.Name = "Ping-Thread :" & ip
                        t.IsBackground = True
                        t.Start()
                    End If
                Next
            Next
        Next
        AddHandler ScanTimer.Tick, AddressOf timertick
        ScanTimer.Interval = 400
        ScanTimer.Start()
    End Sub
    Private Sub timertick()
        ScanTimer.Tag += 1 'count for timeout

        RaiseEvent ProgressUpdated(pingstorage.ips, (pingstorage.Count / pingstorage.aimedsize) * 100) 'calculate progress by dividing finished pings by the aimed number of pings
        pingstorage.ips.Clear() 'empty list again...
        If pingstorage.Count + pingstorage.ips.Count = pingstorage.aimedsize Or ScanTimer.Tag > 75 Then ' if all trys are done
            ScanTimer.Stop()  'stop the timer
            RaiseEvent FinishedScan()
        End If
        FlushMemory() 'flush memory to avoid overflows and keep the memory down
    End Sub
    ''' <summary>
    ''' Sends a ping to a given IpAddress or hostname and adds a hostentry to the shared variable ips if the ping is successful
    ''' </summary>
    ''' <param name="IpOrName">The IP or Hostname to send the Ping to </param>
    ''' <remarks>as</remarks>
    ''' 
    Private Sub sendping(IpOrName As String)
        On Error Resume Next
        If My.Computer.Network.Ping(IpOrName) Then
            pingstorage.ips.Add(IpOrName) 'add hostentry to shared list
        End If
        pingstorage.Count += 1 'add a count to shared integer
    End Sub
    ''' <summary>
    ''' splits an ip into a bytearray
    ''' </summary>
    ''' <param name="ip">the ip to Split</param>
    ''' <returns>The resulting array</returns>
    ''' <remarks></remarks>
    Private Function SplitIp(ip As String) As Byte()
        Dim ipstringarray() As String = Split(ip, ".") 'split ip into array

        Dim ipintarray(3) As Byte
        For i = 0 To ipstringarray.Count - 1
            ipintarray(i) = CByte(ipstringarray(i)) 'convert and store in bytearray
        Next
        Return ipintarray
    End Function
    Private Class pingstorage
        Public Shared ips As New List(Of String)
        Public Shared Count As Integer
        Public Shared aimedsize As Integer
    End Class
End Module
