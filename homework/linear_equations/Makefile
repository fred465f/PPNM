all: Out.txt Out.times.gnuplot.png

# Path to external self-made libraries.
LIBSPATH = ../../Libraries/

# Image file.
Out.times.gnuplot.png : Out.times.txt
	echo '\
			set terminal png background "white" ;\
			set out "$@" ;\
			set xlabel "Size of square matrix" ;\
			set ylabel "QR-decomp. runtime (s)" ;\
			set grid ;\
			set title "QR-decomp. runtime vs. square matrix size" ;\
			plot "$<" with lines notitle ;\
		' | gnuplot

# Main command-line output.
Out.txt : main.exe
	mono $< > $@

# Data
Out.times.txt : resources.exe
	>$@
	for N in $$(seq 100 5 500); do \
		time --format "$$N %e" --output $@ --append \
		mono $< -size:$$N;\
	done

# Executables.
main.exe : main.cs LinearAlgebra.dll
	mcs -target:exe -out:$@ -reference:LinearAlgebra.dll $<
resources.exe : resources.cs LinearAlgebra.dll
	mcs -target:exe -out:$@ -reference:LinearAlgebra.dll $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)LinearAlgebra.cs
	mcs -target:library -out:$@ $<

# Rule to clean.
clean :
	$(RM) *.exe Out.* *.dll *.svg