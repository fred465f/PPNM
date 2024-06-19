all : Out.resourcesSVD.data.txt Out.testSVD.txt

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/MachineLearning/
LIBSPATH3 = ../../Libraries/DataStructures/
LIBSPATH4 = ../../Libraries/Calculus/

# Images.

# Data.
Out.resourcesSVD.data.txt : resourcesSVD.exe
	mono $< > $@

# Main commandline output.
Out.testSVD.txt : testSVD.exe
	mono $< > $@

# Executables.
testSVD.exe : testSVD.cs Library.dll
	mcs -target:exe -out:$@ -reference:Library.dll $<
resourcesSVD.exe : resourcesSVD.cs Library.dll
	mcs -target:exe -out:$@ -reference:Library.dll $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs SVD.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll