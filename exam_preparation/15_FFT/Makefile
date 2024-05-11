all : Out.testFFT.txt

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/Calculus/

# Images.

# Data.

# Main commandline output.
Out.testFFT.txt : testFFT.exe
	mono $< > $@

# Executables.
testFFT.exe : testFFT.cs Library.dll
	mcs -target:exe -reference:Library.dll -reference:System.Numerics.dll -out:$@ $<

# Libraries.
Library.dll : $(LIBSPATH1)FFT.cs
	mcs -target:library -reference:System.Numerics.dll -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll