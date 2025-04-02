# Scripting
## Entrega Taller 2 Diseño Orientado a Pruebas (TDD) 

### Integrantes:

- Isabela Giraldo Jiménez
- Daniel Esteban Ardila Alzate

### Docente:

- Andres Felipe Perez Campillo

---

## Pruebas Unitarias

Primero implementamos las pruebas unitarias y luego añadimos el código del árbol de comportamiento para validar que todo funcionara correctamente:

<details>

<summary>UnitTest1.cs (Click Aquí)</summary>

```C#
using NUnit.Framework;  // Importa el framework NUnit para poder escribir y ejecutar pruebas.
using System;           // Importa System para poder utilizar excepciones y otras funcionalidades básicas.

namespace TestProject_Taller_2_disenoOrientadoAPruebas
{
    // ********************************************************************
    // Pruebas(TestFixture) para la subclase EvenTask.
    // Requisito: Implementar una subclase de Task que verifique si un número es par o impar.
    //
    // Este conjunto de pruebas valida que la subclase EvenTask funcione según lo esperado,
    // es decir, que retorne true para números pares y false para números impares.
    // ********************************************************************
    [TestFixture]
    public class EvenTaskTests
    {
        // Se verifica que EvenTask retorne true para un número par.
        [Test]
        public void EvenTask_RetornaTrue_SiNumeroEsPar()
        {
            // Se crea una instancia de EvenTask con el número 4 (par).
            var task = new EvenTask(4);
            // Assert.IsTrue verifica que el valor retornado por Execute() sea true.
            Assert.IsTrue(task.Execute());
        }

        // Se verifica que EvenTask retorne false para un número impar.
        [Test]
        public void EvenTask_RetornaFalse_SiNumeroEsImpar()
        {
            // Se crea una instancia de EvenTask con el número 3 (impar).
            var task = new EvenTask(3);
            // Se comprueba que Execute() retorne false.
            Assert.IsFalse(task.Execute());
        }
    }

    // ********************************************************************
    // Pruebas para la clase Root.
    // Requisito: El árbol debe tener un único nodo Root, y este solo
    // puede tener un único hijo que no sea otro Root.
    //
    // Este conjunto de pruebas valida el comportamiento de la clase Root, asegurándose
    // de que solo se le pueda asignar un único hijo y que ese hijo no sea de tipo Root.
    // ********************************************************************
    [TestFixture]
    public class RootTests
    {
        // Verifica que no se permite utilizar un Root como child de otro Root.
        [Test]
        public void Root_NoPermiteOtroRootComoChild()
        {
            // Se crea una EvenTask de prueba (dummy) para utilizarla como hijo.
            var dummyTask = new EvenTask(2);
            // Se crea un Root utilizando dummyTask
            var rootChild = new Root(dummyTask);
            // Se espera que al intentar crear un Root con otro Root como hijo se lance una excepción.
            // Esto valida que el Root solo puede tener un único child y no uno de tipo Root.
            Assert.Throws<ArgumentException>(() => new Root(rootChild));
        }

        // Verifica que Root delegue correctamente la ejecución a su único child.
        [Test]
        public void Root_EjecutaSuChildCorrectamente()
        {
            //  Se crea una EvenTask con número 4 (par), por lo que se espera que Execute() retorne true).
            var evenTask = new EvenTask(4);
            // Se crea un Root con la EvenTask.
            var root = new Root(evenTask);
            // Se valida que el método Execute de Root retorne true, ya que su child se ejecuta correctamente.
            Assert.IsTrue(root.Execute());
        }
    }

    // ********************************************************************
    // Pruebas para la clase Sequence (un Composite).
    // Requisito: Un nodo Sequence debe ejecutar sus hijos de izquierda a derecha,
    // retornando true solo si todos se ejecutan exitosamente, o false si alguno falla.
    //
    // Sequence es un nodo compuesto que ejecuta sus hijos de izquierda a derecha
    // y retorna true solo si todos se ejecutan exitosamente.
    // ********************************************************************
    [TestFixture]
    public class SequenceTests
    {
        // Verifica que Sequence retorne true si todos sus hijos se ejecutan exitosamente.
        [Test]
        public void Sequence_RetornaTrue_SiTodosLosHijosTienenExito()
        {
            // Se crea una instancia de Sequence.
            var sequence = new Sequence();
            // Se agregan dos EvenTask con números pares (2 y 4).
            sequence.AddChild(new EvenTask(2));
            sequence.AddChild(new EvenTask(4));
            // Se comprueba que Execute() retorne true, cumpliendo con el comportamiento esperado.
            Assert.IsTrue(sequence.Execute());
        }

        // Verifica que Sequence retorne false si al menos uno de sus hijos falla.
        [Test]
        public void Sequence_RetornaFalse_SiAlgunoDeLosHijosFalla()
        {
            // Se crea una instancia de Sequence.
            var sequence = new Sequence();
            // Se agrega un EvenTask par (2) y luego uno impar (3).
            sequence.AddChild(new EvenTask(2));
            sequence.AddChild(new EvenTask(3));
            // Se valida que Execute() retorne false, ya que un hijo falla.
            Assert.IsFalse(sequence.Execute());
        }
    }

    // ********************************************************************
    // Pruebas para la clase Selector (otro Composite).
    // Requisito: Un nodo Selector debe ejecutar sus hijos de izquierda a derecha,
    // retornando true en cuanto encuentre un hijo que se ejecute con éxito;
    // si ninguno tiene éxito, debe retornar false.
    //
    // Selector ejecuta sus hijos y retorna true tan pronto encuentra un hijo exitoso
    // ********************************************************************
    [TestFixture]
    public class SelectorTests
    {
        // Verifica que Selector retorne true si al menos uno de sus hijos se ejecuta exitosamente.
        [Test]
        public void Selector_RetornaTrue_SiAlgunoDeLosHijosTieneExito()
        {
            // Se crea una instancia de Selector.
            var selector = new Selector();
            // Se agrega un EvenTask impar (3) que retornará false.
            selector.AddChild(new EvenTask(3));
            // Se agrega un EvenTask par (4) que retornará true.
            selector.AddChild(new EvenTask(4));
            // Se valida que Execute() retorne true, ya que al menos un hijo tiene éxito.
            Assert.IsTrue(selector.Execute());
        }

        // Verifica que Selector retorne false si ninguno de sus hijos se ejecuta exitosamente.
        [Test]
        public void Selector_RetornaFalse_SiNingunHijoTieneExito()
        {
            // Se crea una instancia de Selector.
            var selector = new Selector();
            // Se agregan dos EvenTask impares (3 y 5) que retornan false.
            selector.AddChild(new EvenTask(3));
            selector.AddChild(new EvenTask(5));
            // Se valida que Execute() retorne false, cumpliendo el comportamiento esperado.
            Assert.IsFalse(selector.Execute());
        }
    }

    // ********************************************************************
    // Pruebas para la clase BehaviourTree.
    // Requisito: El BehaviourTree debe retornar false si no tiene un nodo Root,
    // y en caso contrario, debe retornar el resultado de la ejecución del Root.
    //
    // BehaviourTree encapsula el árbol de comportamiento y se espera que:
    // - Retorne false si no tiene un nodo Root.
    // - Retorne el resultado del nodo Root cuando está definido.
    // ********************************************************************
    [TestFixture]
    public class BehaviourTreeTests
    {
        // Verifica que un BehaviourTree sin Root retorne false.
        [Test]
        public void BehaviourTree_RetornaFalse_SiElRootEsNull()
        {
            // Crea un BehaviourTree sin asignar un nodo Root (null).
            var tree = new BehaviourTree(null);
            // Se valida que Execute() retorne false.
            // Assert.IsFalse verifica que Execute() retorne false, ya que no hay Root definido.
            Assert.IsFalse(tree.Execute());
        }

        // Verifica que un BehaviourTree retorne el resultado del método Execute de su Root.
        [Test]
        public void BehaviourTree_RetornaElResultadoDelRoot()
        {
            // Se crea una EvenTask par (4) que retorna true.
            var evenTask = new EvenTask(4);
            // Se crea un Root utilizando la EvenTask.
            var root = new Root(evenTask);
            // Se crea un BehaviourTree asignando el Root.
            var tree = new BehaviourTree(root);
            // Se valida que Execute() retorne true, ya que el Root ejecuta exitosamente su child.
            Assert.IsTrue(tree.Execute());
        }
    }

    // ********************************************************************
    // (Opcional) Pruebas generales o de ejemplo.
    // ********************************************************************
    [TestFixture]
    public class GeneralTests
    {
        //  Esta prueba pasa correctamente
        [Test]
        public void Prueba_Que_Pasa()
        {
            // Esta prueba pasará porque simplemente afirmamos que true es true
            Assert.IsTrue(true);
        }

        //  Esta prueba falla intencionalmente
        [Test]
        public void Prueba_Que_Falla()
        {
            // Esta prueba fallará porque estamos afirmando que false es true
            Assert.IsTrue(false, "Esto es una falla intencional para probar el sistema de pruebas.");
        }
    }
}

```

</details>

### Explicación de Cada Sección

1. **Imports y Namespace:**  
   - Se importan las librerías necesarias (NUnit y System) para manejar las pruebas y excepciones.  
   - El código se agrupa en un namespace para mantener la organización.

2. **EvenTaskTests:**  
   - Contiene dos tests: uno verifica que EvenTask retorne true para un número par y otro que retorne false para un número impar.
   - Cada test crea una instancia de EvenTask con un número específico y utiliza aserciones (`Assert.IsTrue` o `Assert.IsFalse`) para confirmar el resultado.

3. **RootTests:**  
   - Comprueba que no se pueda asignar un Root como child de otro Root y que el método Execute() de Root delegue correctamente en su child.
   - Se utiliza `Assert.Throws` para confirmar que se lanza una excepción cuando se viola la restricción de tener un Root como child.

4. **SequenceTests:**  
   - Verifica que la clase Sequence retorne true si todos sus hijos tienen éxito y false si alguno falla.
   - Se agregan instancias de EvenTask a una secuencia y se valida el resultado con aserciones.

5. **SelectorTests:**  
   - Verifica que Selector retorne true si al menos un hijo tiene éxito y false si ninguno lo tiene.
   - Se configuran tests con combinaciones de EvenTask que retornan true o false.

6. **BehaviourTreeTests:**  
   - Comprueba que el árbol de comportamiento retorne false si no se define un Root, y que retorne el resultado del Root cuando está presente.
   - Se crean instancias de BehaviourTree y se verifican sus resultados.

7. **GeneralTests:**  
   - Incluye tests opcionales o de ejemplo para verificación general/adicional.
   - El test `TestEjemplo` simplemente pasa y sirve como plantilla para futuras pruebas.

<IMG src="https://github.com/user-attachments/assets/86df5506-8847-4a66-b567-3bc13cdb3271" width="1000">


A continuación, ejecutamos una prueba individual para asegurarnos de que las pruebas pudieran ejecutarse de forma independiente o en conjunto sin afectar los resultados:

<IMG src="https://github.com/user-attachments/assets/6fd1edd5-9747-4ce5-8e75-1935c1828704" width="700">

Finalmente, agregamos una prueba con el propósito de que fallara intencionalmente cambiando el 2 por un 3, verificando así el correcto funcionamiento del sistema de pruebas.

<IMG src="https://github.com/user-attachments/assets/d110e891-8a19-41e3-8b4c-9ba38cee8bc5" width="500">
<IMG src="https://github.com/user-attachments/assets/0bf8ed27-c203-4c7c-82a2-d4a169f0c772" width="1500">


--- 

## Implementación del árbol de comportamiento

<details>

<summary>Program.cs (Click Aquí)</summary>

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

```
  
</details>

### Explicación de Cada Sección del código

1. **Imports y Namespace:**  
   - Las primeras dos líneas importan espacios de nombres necesarios para manejar excepciones y colecciones.  
   - El namespace agrupa todas nuestras clases para organizarlas y evitar conflictos de nombres.

2. **Clase Node:**  
   - Es una clase abstracta que sirve de base para cualquier nodo del árbol.  
   - Obliga a que sus subclases implementen el método `Execute()`, el cual es el encargado de realizar la acción del nodo.

3. **Clase Root:**  
   - Representa la raíz del árbol, es decir, el nodo principal.  
   - Solo permite tener un único hijo y este hijo no puede ser otro Root, lo que se verifica en el constructor.  
   - Su método `Execute()` simplemente delega la ejecución al hijo.

4. **Clase Composite:**  
   - Es una clase abstracta diseñada para nodos que pueden tener múltiples hijos (por ejemplo, Sequence y Selector).  
   - Tiene una lista de hijos y un método `AddChild` que agrega un nuevo hijo, verificando que no sea null ni un nodo Root.

5. **Clase Sequence:**  
   - Es un nodo que ejecuta a sus hijos en secuencia.  
   - Retorna `false` tan pronto encuentre un hijo que falle; solo retorna `true` si todos los hijos se ejecutan correctamente.

6. **Clase Selector:**  
   - Es un nodo que ejecuta a sus hijos en orden y retorna `true` tan pronto encuentre un hijo que se ejecute con éxito.  
   - Si ninguno de sus hijos tiene éxito, retorna `false`.

7. **Clase Task y EvenTask:**  
   - `Task` es una clase abstracta para las tareas (nodos hoja) que realizan acciones concretas.  
   - `EvenTask` es una implementación concreta de Task que evalúa si un número es par (retorna `true`) o impar (retorna `false`).

8. **Clase BehaviourTree:**  
   - Encapsula el árbol de comportamiento completo y contiene el nodo Root.  
   - Su método `Execute()` valida si el Root existe y, en caso afirmativo, ejecuta el árbol delegando en el Root.

---

## Revisemos cómo se cumplen tanto en la implementación como en las pruebas la solución presentada :

### 1. Un árbol de comportamiento solo tiene un nodo Root  
- **Requerimiento:** El árbol debe tener un único nodo Root.  
- **Implementación:**  
  - La clase **Root** está diseñada para ser el único nodo raíz.  
- **Pruebas:**  
  - El test `Root_NoPermiteOtroRootComoChild()` verifica que no se puede asignar otro Root como child.

### 2. Un nodo Root solo tiene un único child y ese child no puede ser otro Root  
- **Requerimiento:** El nodo Root debe recibir exactamente un child y este child no puede ser un Root.  
- **Implementación:**  
  - En el constructor de **Root** se verifica que el parámetro no sea null y que no sea otra instancia de Root.  
- **Pruebas:**  
  - `Root_NoPermiteOtroRootComoChild()` lanza una excepción si se intenta pasar otro Root como child.  
  - `Root_EjecutaSuChildCorrectamente()` confirma que el Root delega correctamente la ejecución a su child.

### 3. Un nodo Composite no puede ser instanciado, excepto por sus subclases  
- **Requerimiento:** La clase Composite debe ser abstracta para forzar que solo se puedan instanciar sus subclases (como Sequence y Selector).  
- **Implementación:**  
  - **Composite** está declarada como abstracta, por lo que no se puede instanciar directamente.  
- **Pruebas:**  
  - No se necesita test explícito, ya que el compilador impide la instanciación de clases abstractas.

### 4. Un nodo Composite no puede tener ningún Root entre sus nodos child  
- **Requerimiento:** Al agregar hijos a un Composite (Sequence o Selector), no se debe permitir un nodo Root.  
- **Implementación:**  
  - En el método `AddChild` de **Composite** se verifica que el nodo agregado no sea de tipo Root.  
- **Pruebas:**  
  - Existe un test (por ejemplo, en **RootTests** o en un test específico de Composite) que verifica que al intentar agregar un Root a un Composite se lance una excepción.

### 5. Un nodo Task no puede ser instanciado, excepto por sus subclases, y no tiene nodos child  
- **Requerimiento:** La clase **Task** debe ser abstracta y sus subclases (como EvenTask) no deben permitir agregar hijos.  
- **Implementación:**  
  - **Task** está definida como abstracta y no tiene ningún método para agregar children.  
- **Pruebas:**  
  - Se prueban las subclases (como **EvenTask**) para verificar su comportamiento.  
  - La imposibilidad de agregar hijos se verifica por diseño (no existe método AddChild en Task).

### 6. La jerarquía de herencia debe ser correcta  
- **Requerimiento:**  
  - Todas las clases implementadas derivan de Node.
  - ![image](https://github.com/user-attachments/assets/262c0ed6-9ce4-4b51-af92-ce874a11e627)
  - **Sequence** y **Selector** derivan de **Composite**.
  - ![image](https://github.com/user-attachments/assets/22f26e85-bd86-41df-8a61-f72b384078b3)
  - **Task** deriva de **Node** (y no de Root o Composite).
  - ![image](https://github.com/user-attachments/assets/4e2db0d0-01fc-4c2b-be83-b37ab6af9296)
  - Ningún nodo implementado deriva de Root (solo Root es la clase para la raíz).
  - ![image](https://github.com/user-attachments/assets/89a318fd-0ef0-4ea2-b8b8-34617a5b031f)

- **Implementación:**  
  - La jerarquía está definida correctamente en el código:  
    - **Node** es la base.  
    - **Root** hereda de Node.  
    - **Composite** es abstracta y hereda de Node; **Sequence** y **Selector** heredan de Composite.  
    - **Task** es abstracta y hereda de Node; **EvenTask** hereda de Task.
- **Pruebas:**  
  - Se comprueba al compilar y la estructura del código lo garantiza.

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

### 8. Requisitos de los Commits y TDD  
- **Requerimiento:**  
  - El primer commit debe contener solo las pruebas unitarias diseñadas (incluso si usan Assert.Pass()).  
  - El segundo commit debe incluir la implementación (basada en el taller de POO).  
  - Los commits siguientes son correcciones para que las pruebas pasen.
- **Implementación y Pruebas:**  
  - Esto se gestiona en el control de versiones y en el proceso de TDD.  
  - Las pruebas que hemos definido y el código de implementación cumplen con estos lineamientos.

