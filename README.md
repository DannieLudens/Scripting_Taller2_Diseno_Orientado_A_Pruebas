Integrantes:
Daniel Ardila Alzate
Isabela Giraldo Jiménez

Revisemos cómo se cumplen tanto en la implementación como en las pruebas:

---

### 1. Un árbol de comportamiento solo tiene un nodo Root  
- **Requerimiento:** El árbol debe tener un único nodo Root.  
- **Implementación:**  
  - La clase **Root** está diseñada para ser el único nodo raíz.  
- **Pruebas:**  
  - El test `Root_NoPermiteOtroRootComoChild()` verifica que no se puede asignar otro Root como child.

---

### 2. Un nodo Root solo tiene un único child y ese child no puede ser otro Root  
- **Requerimiento:** El nodo Root debe recibir exactamente un child y este child no puede ser un Root.  
- **Implementación:**  
  - En el constructor de **Root** se verifica que el parámetro no sea null y que no sea otra instancia de Root.  
- **Pruebas:**  
  - `Root_NoPermiteOtroRootComoChild()` lanza una excepción si se intenta pasar otro Root como child.  
  - `Root_EjecutaSuChildCorrectamente()` confirma que el Root delega correctamente la ejecución a su child.

---

### 3. Un nodo Composite no puede ser instanciado, excepto por sus subclases  
- **Requerimiento:** La clase Composite debe ser abstracta para forzar que solo se puedan instanciar sus subclases (como Sequence y Selector).  
- **Implementación:**  
  - **Composite** está declarada como abstracta, por lo que no se puede instanciar directamente.  
- **Pruebas:**  
  - No se necesita test explícito, ya que el compilador impide la instanciación de clases abstractas.

---

### 4. Un nodo Composite no puede tener ningún Root entre sus nodos child  
- **Requerimiento:** Al agregar hijos a un Composite (Sequence o Selector), no se debe permitir un nodo Root.  
- **Implementación:**  
  - En el método `AddChild` de **Composite** se verifica que el nodo agregado no sea de tipo Root.  
- **Pruebas:**  
  - Existe un test (por ejemplo, en **RootTests** o en un test específico de Composite) que verifica que al intentar agregar un Root a un Composite se lance una excepción.

---

### 5. Un nodo Task no puede ser instanciado, excepto por sus subclases, y no tiene nodos child  
- **Requerimiento:** La clase **Task** debe ser abstracta y sus subclases (como EvenTask) no deben permitir agregar hijos.  
- **Implementación:**  
  - **Task** está definida como abstracta y no tiene ningún método para agregar children.  
- **Pruebas:**  
  - Se prueban las subclases (como **EvenTask**) para verificar su comportamiento.  
  - La imposibilidad de agregar hijos se verifica por diseño (no existe método AddChild en Task).

---

### 6. La jerarquía de herencia debe ser correcta  
- **Requerimiento:**  
  - Todas las clases implementadas derivan de Node.  
  - **Sequence** y **Selector** derivan de **Composite**.  
  - **Task** deriva de **Node** (y no de Root o Composite).  
  - Ningún nodo implementado deriva de Root (solo Root es la clase para la raíz).  
- **Implementación:**  
  - La jerarquía está definida correctamente en el código:  
    - **Node** es la base.  
    - **Root** hereda de Node.  
    - **Composite** es abstracta y hereda de Node; **Sequence** y **Selector** heredan de Composite.  
    - **Task** es abstracta y hereda de Node; **EvenTask** hereda de Task.
- **Pruebas:**  
  - No es necesario un test específico para la jerarquía, ya que se comprueba al compilar y la estructura del código lo garantiza.

---

### 7. Comportamiento del método Execute()  
- **Requerimiento:**  
  - Para un Root vacío, `Execute()` debe retornar false.  
  - Para un Root con un Task, `Execute()` retorna según el resultado del Task (true si la tarea se completa, false si no).  
  - **Sequence** retorna true solo si todos sus hijos tienen éxito, y false si alguno falla.  
  - **Selector** retorna true si al menos un hijo tiene éxito y false si ninguno lo tiene.
- **Implementación:**  
  - Cada clase implementa su versión de `Execute()` de acuerdo a la lógica requerida.  
  - **EvenTask** retorna true si el número es par.  
  - **Sequence** recorre sus hijos y retorna false en el primer fallo.  
  - **Selector** recorre sus hijos y retorna true tan pronto encuentra uno exitoso.  
  - **BehaviourTree** delega la ejecución en su Root, y si la propiedad Root es null, retorna false.
- **Pruebas:**  
  - `EvenTaskTests` verifica la paridad (números pares/ímpares).  
  - `SequenceTests` y `SelectorTests` prueban los comportamientos descritos.  
  - `BehaviourTreeTests` comprueba que se retorna false cuando Root es null y que se obtiene el resultado del Root cuando está definido.

---

### 8. Requisitos de los Commits y TDD  
- **Requerimiento:**  
  - El primer commit debe contener solo las pruebas unitarias diseñadas (incluso si usan Assert.Pass()).  
  - El segundo commit debe incluir la implementación (basada en el taller de POO).  
  - Los commits siguientes son correcciones para que las pruebas pasen.
- **Implementación y Pruebas:**  
  - Esto se gestiona en el control de versiones y en el proceso de TDD, no en el código en sí.  
  - Las pruebas que hemos definido y el código de implementación cumplen con estos lineamientos.

---

### Conclusión

Nuestra implementación y conjunto de pruebas abordan cada uno de los puntos solicitados en el Taller Práctico 2:

- La estructura del árbol con un único Root y las restricciones en la composición de nodos se validan en los tests de **Root**, **Sequence** y **Selector**.
- La validación de la lógica de una tarea (EvenTask) se verifica en los tests correspondientes.
- La jerarquía y la estructura de clases se definen correctamente en el código, lo que se verifica indirectamente.
- El comportamiento global del árbol a través de la clase **BehaviourTree** se prueba mediante sus tests.

## Pruebas Unitarias

Primero implementamos las pruebas unitarias y luego añadimos el código del árbol de comportamiento para validar que todo funcionara correctamente:


![image](https://github.com/user-attachments/assets/e5c83cfa-5d34-43a4-9ec8-cb9806009f26)


A continuación, ejecutamos una prueba individual para asegurarnos de que las pruebas pudieran ejecutarse de forma independiente o en conjunto sin afectar los resultados:

![image](https://github.com/user-attachments/assets/6fd1edd5-9747-4ce5-8e75-1935c1828704)


Finalmente, agregamos una prueba con el propósito de que fallara intencionalmente, verificando así el correcto funcionamiento del sistema de pruebas.


![image](https://github.com/user-attachments/assets/1b8df89f-9e95-4772-b10a-0adbdab201e4)

<IMG src="https://github.com/user-attachments/assets/1b8df89f-9e95-4772-b10a-0adbdab201e4" width="350">

<details>

<summary>Codigo fuente (Click Aquí)</summary>

```c#
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

```
  
</details>
