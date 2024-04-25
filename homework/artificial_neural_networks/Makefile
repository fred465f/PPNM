all : Out.testNeuralNetwork.txt

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/MachineLearning/
LIBSPATH3 = ../../Libraries/DataStructures/

# Images.

# Data.

# Main commandline output.
Out.testNeuralNetwork.txt : testNeuralNetwork.exe
	mono $< > $@

# Executables.
testNeuralNetwork.exe : testNeuralNetwork.cs Library.dll
	mcs -target:exe -out:$@ -reference:Library.dll $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH2)Minimization.cs $(LIBSPATH2)NeuralNetworks.cs $(LIBSPATH3)GenericList.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll