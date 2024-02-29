all: Out.txt

# Path to external self-made libraries.
LIBSPATH = ../../Libraries/

# Image files.
# ...

# Data for image files.
# ...

# Main output to commandline
Out.txt : main.exe
	mono $< > $@

# Executables.
main.exe : main.cs leastSquares.dll LinearAlgebra.dll
	mcs -target:exe -reference:leastSquares.dll -reference:LinearAlgebra.dll -out:$@ $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)LinearAlgebra.cs
	mcs -target:library -out:$@ $<
leastSquares.dll : leastSquares.cs LinearAlgebra.dll
	mcs -target:library -reference:LinearAlgebra.dll -out:$@ $<

# Rule to clean.
clean :
	$(RM) *.exe [Oo]ut.* *.dll