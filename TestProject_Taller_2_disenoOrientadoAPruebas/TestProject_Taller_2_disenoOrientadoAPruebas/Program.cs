using System;                           // Importa clases básicas y manejo de excepciones.
using System.Collections.Generic;       // Importa colecciones genéricas, como List<T>.

namespace TestProject_Taller_2_disenoOrientadoAPruebas
{
    // --------------------------------------------------------------------------
    // Clase Node: Base abstracta para todos los nodos del árbol de comportamiento.
    // --------------------------------------------------------------------------
    public abstract class Node
    {
        // Método abstracto que cada nodo debe implementar.
        // Retorna true si la acción del nodo se ejecuta exitosamente.
        public abstract bool Execute();
    }

    // --------------------------------------------------------------------------
    // Clase Root: Representa el nodo raíz del árbol.
    // Solo puede tener un único hijo, y ese hijo no puede ser otro Root.
    // --------------------------------------------------------------------------
    public class Root : Node
    {
        private Node child; // Almacena el único nodo hijo.

        // Constructor que recibe un nodo hijo.
        public Root(Node child)
        {
            // Si el child es null, lanza una excepción.
            if (child == null)
                throw new ArgumentNullException(nameof(child), "El child no puede ser null.");
            // Si el child es otro Root, lanza una excepción.
            if (child is Root)
                throw new ArgumentException("El nodo hijo no puede ser otro Root.");
            this.child = child; // Asigna el child.
        }

        // El método Execute delega la ejecución al único hijo.
        public override bool Execute()
        {
            return child.Execute();
        }
    }

    // --------------------------------------------------------------------------
    // Clase Composite: Abstracta para nodos que pueden tener múltiples hijos.
    // Ejemplos: Sequence y Selector.
    // --------------------------------------------------------------------------
    public abstract class Composite : Node
    {
        // Lista de nodos hijos.
        protected List<Node> children = new List<Node>();

        // Método para agregar un nodo hijo.
        public void AddChild(Node child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child), "El child no puede ser null.");
            if (child is Root)
                throw new ArgumentException("No se permite agregar un nodo Root como hijo.");
            children.Add(child);  // Agrega el nodo a la lista.
        }
    }

    // --------------------------------------------------------------------------
    // Clase Sequence: Ejecuta cada nodo hijo en orden.
    // Retorna true solo si TODOS los hijos se ejecutan exitosamente.
    // --------------------------------------------------------------------------
    public class Sequence : Composite
    {
        public override bool Execute()
        {
            foreach (var child in children)
            {
                // Si algún hijo falla, retorna false inmediatamente.
                if (!child.Execute())
                    return false;
            }
            // Si todos los hijos se ejecutaron correctamente, retorna true.
            return true;
        }
    }

    // --------------------------------------------------------------------------
    // Clase Selector: Ejecuta cada nodo hijo en orden.
    // Retorna true tan pronto encuentre un hijo que se ejecute con éxito.
    // --------------------------------------------------------------------------
    public class Selector : Composite
    {
        public override bool Execute()
        {
            foreach (var child in children)
            {
                // Si encuentra un hijo exitoso, retorna true.
                if (child.Execute())
                    return true;
            }
            // Si ninguno tiene éxito, retorna false.
            return false;
        }
    }

    // --------------------------------------------------------------------------
    // Clase Task: Abstracta para tareas (nodos hoja) que no tienen hijos.
    // --------------------------------------------------------------------------
    public abstract class Task : Node
    {
        // Aquí se puede incluir lógica o propiedades comunes para todas las tareas.
    }

    // --------------------------------------------------------------------------
    // Clase EvenTask: Subclase de Task que verifica si un número es par.
    // Retorna true si es par y false si es impar.
    // --------------------------------------------------------------------------
    public class EvenTask : Task
    {
        private int number; // Número a evaluar.

        // Constructor que recibe el número.
        public EvenTask(int number)
        {
            this.number = number;
        }

        // Ejecuta la tarea: retorna true si el número es par, false si es impar.
        public override bool Execute()
        {
            return number % 2 == 0;
        }
    }

    // --------------------------------------------------------------------------
    // Clase BehaviourTree: Encapsula el árbol de comportamiento.
    // Contiene el nodo Root y un método para ejecutar el árbol.
    // --------------------------------------------------------------------------
    public class BehaviourTree
    {
        public Root? Root { get; set; }  // Propiedad para almacenar el nodo raíz.

        // Constructor que inicializa el árbol con un Root.
        public BehaviourTree(Root? root)
        {
            Root = root;
        }

        // Ejecuta el árbol: si el Root es null, retorna false; de lo contrario, ejecuta el Root.
        public bool Execute()
        {
            if (Root == null)
                return false;
            return Root.Execute();
        }
    }

    
}
