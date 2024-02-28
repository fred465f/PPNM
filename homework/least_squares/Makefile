all: Out.txt

# Path to LinearAlgebra library.
LIBSPATH = ../../Libraries/LinearAlgebra/

# Image files.
# ...

# Data for image files.
# ...

# Main output to commandline
Out.txt : main.exe
	mono $< > $@

# Executables.
main.exe : main.cs matrix.dll
	mcs -target:exe -reference:matrix.dll -out:$@ $<

# Libraries.
matrix.dll: $(LIBSPATH)matrix.cs $(LIBSPATH)vector.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean :
	$(RM) *.exe [Oo]ut.* *.dll