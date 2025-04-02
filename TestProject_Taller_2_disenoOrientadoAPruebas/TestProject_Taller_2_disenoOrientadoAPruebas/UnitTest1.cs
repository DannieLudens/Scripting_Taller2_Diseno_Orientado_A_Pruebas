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
