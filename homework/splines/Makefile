all: Out.txt

# Path to external self-made libraries.
LIBSPATH = ../../Libraries/

# Image files.
# ...

# Main output to commandline
Out.txt : main.exe
	mono $< > $@

# Executables.
main.exe : main.cs LinearAlgebra.dll splines.dll
	mcs -target:exe -reference:LinearAlgebra.dll -reference:splines.dll -out:$@ $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)LinearAlgebra.cs
	mcs -target:library -out:$@ $<
splines.dll : splines.cs LinearAlgebra.dll
	mcs -target:library -reference:LinearAlgebra.dll -out:$@ $<

# Rule to clean.
clean :
	$(RM) *.exe [Oo]ut.* *.dll