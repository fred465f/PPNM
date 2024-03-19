all : Out.testRootFinder.txt Out.shootingMethodHydrogenAtomData.txt

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/
LIBSPATH2 = ../../Libraries/Calculus/
LIBSPATH3 = ../../Libraries/DataStructures/

# Images.

# Data.
Out.shootingMethodHydrogenAtomData.txt : shootingMethodHydrogenAtom.exe
	mono $< > $@

# Main commandline output.
Out.testRootFinder.txt : testRootFinder.exe
	mono $< > $@

# Executables.
testRootFinder.exe : testRootFinder.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<
shootingMethodHydrogenAtom.exe : shootingMethodHydrogenAtom.cs Library.dll
	mcs -target:exe -reference:Library.dll -out:$@ $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH1)QRGS.cs $(LIBSPATH1)Splines.cs $(LIBSPATH2)RootFinder.cs $(LIBSPATH2)ODE.cs $(LIBSPATH3)GenericList.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll