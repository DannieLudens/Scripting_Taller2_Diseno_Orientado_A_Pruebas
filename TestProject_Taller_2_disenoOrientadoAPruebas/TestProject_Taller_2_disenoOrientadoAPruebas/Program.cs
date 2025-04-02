using System;                           // Importa clases básicas y manejo de excepciones.
using System.Collections.Generic;       // Importa colecciones genéricas, como List<T>.

namespace TestProject_Taller_2_disenoOrientadoAPruebas
{
    // --------------------------------------------------------------------------
    // Clase Node: Base abstracta para todos los nodos del árbol de comportamiento.
    // Esta clase define la plantilla básica para cualquier nodo, obligando a sus subclases
    // a implementar el método Execute() que realiza la acción del nodo.
    // --------------------------------------------------------------------------
    public abstract class Node
    {
        // Método abstracto que cada nodo debe implementar.
        // Retorna true si la acción del nodo se ejecuta exitosamente, false en caso contrario.
        public abstract bool Execute();
    }

    // --------------------------------------------------------------------------
    // Clase Root: Representa el nodo raíz del árbol.
    // Solo puede tener un único hijo, y ese hijo no puede ser otro Root.
    // --------------------------------------------------------------------------
    public class Root : Node
    {
        private Node child; // Variable privada que almacena el único nodo hijo del Root.

        // Constructor de la clase Root.
        // Recibe un nodo (child) que será el único hijo de este Root.
        public Root(Node child)
        {
            // Verifica que el nodo pasado no sea null.
            // Si el child es null, lanza una excepción.
            if (child == null)
                throw new ArgumentNullException(nameof(child), "El child no puede ser null.");
            // Verifica que el nodo pasado no sea de tipo Root.
            // Esto asegura que no se anide un Root dentro de otro Root.
            // Si el child es otro Root, lanza una excepción.
            if (child is Root)
                throw new ArgumentException("El nodo hijo no puede ser otro Root.");
            // Asigna el nodo pasado a la variable privada 'child'.
            this.child = child; 
        }

        // Implementación del método Execute.
        // Este método delega la ejecución a su único nodo hijo y retorna el resultado de esa ejecución.
        public override bool Execute()
        {
            return child.Execute();
        }
    }

    // --------------------------------------------------------------------------
    // Clase Composite: Clase abstracta para nodos que pueden tener múltiples hijos.
    // Se utiliza como base para nodos compuestos como Sequence y Selector.
    // --------------------------------------------------------------------------
    public abstract class Composite : Node
    {
        // Lista protegida que almacena todos los nodos hijos.
        // La usamos para permitir que las clases derivadas (Sequence, Selector) accedan a ella.
        protected List<Node> children = new List<Node>();

        // Método para agregar un nodo hijo.
        public void AddChild(Node child)
        {
            // Verifica que el nodo no sea null.
            if (child == null)
                throw new ArgumentNullException(nameof(child), "El child no puede ser null.");
            // No se permite agregar un nodo Root como hijo de un Composite.
            if (child is Root)
                throw new ArgumentException("No se permite agregar un nodo Root como hijo.");
            // Agrega el nodo a la lista de hijos.
            children.Add(child);
        }
    }

    // --------------------------------------------------------------------------
    // Clase Sequence: Un nodo Composite que ejecuta a cada uno de sus hijos en secuencia.
    // Retorna true solo si todos los hijos se ejecutan exitosamente.
    // --------------------------------------------------------------------------
    public class Sequence : Composite
    {
        // Implementación del método Execute para Sequence.
        public override bool Execute()
        {
            // Itera sobre cada nodo hijo en la lista 'children'.
            foreach (var child in children)
            {
                // Ejecuta el nodo hijo.
                // Si el resultado es false (es decir, falla), retorna false inmediatamente.
                if (!child.Execute())
                    return false;
            }
            // Si todos los hijos se ejecutaron correctamente, retorna true.
            return true;
        }
    }

    // --------------------------------------------------------------------------
    // Clase Selector: Un nodo Composite que ejecuta a sus hijos en orden.
    // Retorna true tan pronto encuentre un hijo que se ejecute con éxito; de lo contrario, false.
    // --------------------------------------------------------------------------
    public class Selector : Composite
    {
        // Implementación del método Execute para Selector.
        public override bool Execute()
        {
            // Itera sobre cada nodo hijo en la lista 'children'.
            foreach (var child in children)
            {
                // Ejecuta el nodo hijo.
                // Si encuentra un hijo que retorna true, retorna true inmediatamente.
                if (child.Execute())
                    return true;
            }
            // Si ninguno de los hijos se ejecuta exitosamente, retorna false.
            return false;
        }
    }

    // --------------------------------------------------------------------------
    // Clase Task: Clase abstracta para representar tareas o nodos hoja.
    // Las tareas son nodos que realizan acciones concretas y no tienen hijos.
    // --------------------------------------------------------------------------
    public abstract class Task : Node
    {
        // Esta clase es abstracta para que solo se puedan crear instancias a través de sus subclases.
        // Se pueden incluir propiedades o lógica común para todas las tareas aquí, si fuera necesario.
    }

    // --------------------------------------------------------------------------
    // Clase EvenTask: Subclase de Task que verifica si un número es par.
    // Retorna true si es par y false si es impar.
    // --------------------------------------------------------------------------
    public class EvenTask : Task
    {
        private int number; // Variable privada que almacena el número a evaluar.

        // Constructor que recibe el número a evaluar.
        public EvenTask(int number)
        {
            // Asigna el número recibido a la variable 'number'.
            this.number = number;
        }

        // Implementación del método Execute para EvenTask.
        // Calcula el módulo del número entre 2 y retorna true si el resultado es 0 (par).
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
        // Se declara como Root? (nullable) para permitir que el árbol no tenga un Root definido.
        public Root? Root { get; set; }  // Propiedad para almacenar el nodo raíz.

        // Constructor que inicializa el BehaviourTree con un nodo Root.
        public BehaviourTree(Root? root)
        {
            // Asigna el nodo Root recibido al árbol.
            Root = root;
        }

        // Método para ejecutar el árbol.
        // Si el Root es null, retorna false; de lo contrario, ejecuta el método Execute del Root.
        public bool Execute()
        {
            if (Root == null)
                return false;
            return Root.Execute();
        }
    }


}
