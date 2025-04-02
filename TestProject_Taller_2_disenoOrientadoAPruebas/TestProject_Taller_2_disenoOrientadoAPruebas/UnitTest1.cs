namespace TestProject_Taller_2_disenoOrientadoAPruebas
{
    // ********************************************************************
    // Pruebas para la subclase EvenTask.
    // Requisito del taller: Implementar una subclase de Task que verifique 
    // si un número es par o impar. Retorna true si es par y false si es impar.
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
            // Se comprueba que Execute() retorne true, cumpliendo con la especificación.
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
    // Requisito del taller: El árbol debe tener un único nodo Root, y este solo
    // puede tener un único hijo que no sea otro Root.
    // ********************************************************************
    [TestFixture]
    public class RootTests
    {
        // Verifica que no se permite utilizar un Root como child de otro Root.
        [Test]
        public void Root_NoPermiteOtroRootComoChild()
        {
            // Se crea una EvenTask dummy para utilizarla.
            var dummyTask = new EvenTask(2);
            // Se crea un Root utilizando la EvenTask.
            var rootChild = new Root(dummyTask);
            // Se espera que al intentar crear un Root con otro Root como hijo se lance una excepción.
            // Esto valida que el Root solo puede tener un único child y no uno de tipo Root.
            Assert.Throws<ArgumentException>(() => new Root(rootChild));
        }

        // Verifica que Root delegue correctamente la ejecución a su único child.
        [Test]
        public void Root_EjecutaSuChildCorrectamente()
        {
            // Se crea una EvenTask que retorna true (porque 4 es par).
            var evenTask = new EvenTask(4);
            // Se crea un Root con la EvenTask.
            var root = new Root(evenTask);
            // Se valida que el método Execute de Root retorne true, ya que su child se ejecuta correctamente.
            Assert.IsTrue(root.Execute());
        }
    }

    // ********************************************************************
    // Pruebas para la clase Sequence (un Composite).
    // Requisito del taller: Un nodo Sequence debe ejecutar sus hijos de izquierda a derecha,
    // retornando true solo si todos se ejecutan exitosamente, o false si alguno falla.
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
    // Requisito del taller: Un nodo Selector debe ejecutar sus hijos de izquierda a derecha,
    // retornando true en cuanto encuentre un hijo que se ejecute con éxito; si ninguno tiene éxito,
    // debe retornar false.
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
    // Requisito del taller: El BehaviourTree debe retornar false si no tiene un nodo Root,
    // y en caso contrario, debe retornar el resultado de la ejecución del Root.
    // ********************************************************************
    [TestFixture]
    public class BehaviourTreeTests
    {
        // Verifica que un BehaviourTree sin Root retorne false.
        [Test]
        public void BehaviourTree_RetornaFalse_SiElRootEsNull()
        {
            // Se crea un BehaviourTree sin asignar un Root (null).
            var tree = new BehaviourTree(null);
            // Se valida que Execute() retorne false.
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
        // Un test simple de ejemplo que siempre pasa.
        [Test]
        public void TestEjemplo()
        {
            // Assert.Pass() indica que la prueba pasa sin realizar ninguna verificación adicional.
            Assert.Pass();
        }
    }
}
