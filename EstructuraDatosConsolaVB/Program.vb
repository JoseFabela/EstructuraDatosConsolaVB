Imports System
Imports System.Collections.Generic

Module Module1

    Sub Main()
        Dim tareasPendientes As New Queue(Of String)()
        Dim tareasCompletadas As New List(Of String)()
        Dim arbolPrioridad As New PriorityTree("Proyecto")
        Dim grafoTareas As New TaskGraph()

        While True
            Console.WriteLine(vbLf & "Menú:")
            Console.WriteLine("1. Agregar tarea pendiente (Cola)")
            Console.WriteLine("2. Completar tarea (Cola y Lista)")
            Console.WriteLine("3. Ver tareas pendientes (Cola)")
            Console.WriteLine("4. Ver tareas completadas (Lista)")
            Console.WriteLine("5. Agregar tarea al árbol de prioridad")
            Console.WriteLine("6. Ver árbol de prioridad")
            Console.WriteLine("7. Agregar tarea al grafo de tareas")
            Console.WriteLine("8. Ver grafo de tareas (BFS)")
            Console.WriteLine("9. Salir")

            Console.Write("Seleccione una opción: ")
            Dim opcion As String = Console.ReadLine()

            Select Case opcion
                Case "1"
                    Console.Write("Ingrese la nueva tarea pendiente: ")
                    Dim nuevaTarea As String = Console.ReadLine()
                    tareasPendientes.Enqueue(nuevaTarea)
                    Console.WriteLine($"Tarea ""{nuevaTarea}"" agregada correctamente a la Cola.")
                Case "2"
                    If tareasPendientes.Count > 0 Then
                        Dim tareaCompleta As String = tareasPendientes.Dequeue()
                        tareasCompletadas.Add(tareaCompleta)
                        Console.WriteLine($"Tarea ""{tareaCompleta}"" completada y movida a Lista de Tareas Completadas.")
                    Else
                        Console.WriteLine("No hay tareas pendientes para completar.")
                    End If
                Case "3"
                    Console.WriteLine(vbLf & "Tareas Pendientes (Cola):")
                    For Each tarea As String In tareasPendientes
                        Console.WriteLine(tarea)
                    Next
                Case "4"
                    Console.WriteLine(vbLf & "Tareas Completadas (Lista):")
                    For Each tarea As String In tareasCompletadas
                        Console.WriteLine(tarea)
                    Next
                Case "5"
                    Console.Write("Ingrese la nueva tarea para el árbol de prioridad: ")
                    Dim nuevaTareaArbol As String = Console.ReadLine()
                    Console.Write("Ingrese la prioridad (Alta, Media, Baja): ")
                    Dim prioridadTareaArbol As String = Console.ReadLine()
                    arbolPrioridad.Insert(nuevaTareaArbol, prioridadTareaArbol)
                    Console.WriteLine($"Tarea ""{nuevaTareaArbol}"" agregada al Árbol de Prioridad.")
                Case "6"
                    Console.WriteLine(vbLf & "Árbol de Prioridad:")
                    arbolPrioridad.InOrderTraversal(arbolPrioridad.Root)
                Case "7"
                    Console.Write("Ingrese la nueva tarea para el grafo de tareas: ")
                    Dim nuevaTareaGrafo As String = Console.ReadLine()
                    grafoTareas.AddTask(nuevaTareaGrafo)
                    Console.WriteLine($"Tarea ""{nuevaTareaGrafo}"" agregada al Grafo de Tareas.")
                Case "8"
                    Console.Write("Ingrese la tarea para iniciar BFS en el grafo de tareas: ")
                    Dim tareaInicioBFS As String = Console.ReadLine()
                    grafoTareas.BFS(tareaInicioBFS)
                Case "9"
                    Console.WriteLine("Saliendo del programa. ¡Hasta luego!")
                    Return
                Case Else
                    Console.WriteLine("Opción no válida. Por favor, ingrese un número del 1 al 9.")
            End Select
        End While
    End Sub

    ' Implementación de árbol de prioridad
    Class PriorityTree
        Public Class Node
            Public Task As String
            Public Priority As String
            Public Left, Right As Node

            Public Sub New(ByVal task As String, ByVal priority As String)
                Me.Task = task
                Me.Priority = priority
                Left = Nothing
                Right = Nothing
            End Sub
        End Class

        Public Root As Node

        Public Sub New(ByVal rootTask As String)
            Root = New Node(rootTask, "Media")
        End Sub

        Public Sub Insert(ByVal task As String, ByVal priority As String)
            Root = InsertRecursive(Root, task, priority)
        End Sub

        Private Function InsertRecursive(ByVal root As Node, ByVal task As String, ByVal priority As String) As Node
            If root Is Nothing Then
                Return New Node(task, priority)
            End If

            If String.Compare(priority, root.Priority) < 0 Then
                root.Left = InsertRecursive(root.Left, task, priority)
            Else
                root.Right = InsertRecursive(root.Right, task, priority)
            End If

            Return root
        End Function

        Public Sub InOrderTraversal(ByVal root As Node)
            If root IsNot Nothing Then
                InOrderTraversal(root.Left)
                Console.WriteLine($"{root.Task} - {root.Priority}")
                InOrderTraversal(root.Right)
            End If
        End Sub
    End Class

    ' Implementación de grafo de tareas
    Class TaskGraph
        Private taskDependencies As New Dictionary(Of String, List(Of String))()

        Public Sub New()
            taskDependencies = New Dictionary(Of String, List(Of String))()
        End Sub

        Public Sub AddTask(ByVal task As String)
            If Not taskDependencies.ContainsKey(task) Then
                taskDependencies(task) = New List(Of String)()
            End If
        End Sub

        Public Sub AddTaskDependency(ByVal task As String, ByVal dependency As String)
            If taskDependencies.ContainsKey(task) Then
                taskDependencies(task).Add(dependency)
            End If
        End Sub

        Public Sub BFS(startTask As String)
            If Not taskDependencies.ContainsKey(startTask) Then
                Console.WriteLine($"La tarea '{startTask}' no existe en el grafo de tareas.")
                Return
            End If

            Dim visited As New HashSet(Of String)()
            Dim queue As New Queue(Of String)()

            visited.Add(startTask)
            queue.Enqueue(startTask)

            While queue.Count > 0
                Dim currentTask As String = queue.Dequeue()
                Console.WriteLine(currentTask)

                If taskDependencies.ContainsKey(currentTask) Then
                    For Each dependency As String In taskDependencies(currentTask)
                        If Not visited.Contains(dependency) Then
                            visited.Add(dependency)
                            queue.Enqueue(dependency)
                        End If
                    Next
                End If
            End While
        End Sub
    End Class

End Module
