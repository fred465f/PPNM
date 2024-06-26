all: Out.txt Out.times.gnuplot.png

# Path to external self-made libraries.
LIBSPATH = ../../Libraries/LinearAlgebra/

# Image file.
Out.times.gnuplot.png : Out.times.txt
	echo '\
			set terminal png background "white" ;\
			set out "$@" ;\
			set xlabel "Size of square matrix" ;\
			set ylabel "QR-decomp. runtime (s)" ;\
			set grid ;\
			set title "QR-decomp. runtime vs. square matrix size" ;\
			f(x) = b + (x/a)**c ;\
			b = 1; a = 90; c = 2 ;\
			fit f(x) "$<" via a, b, c ;\
			plot "$<" with lines lw 3 linecolor rgb "#4BB3E1" title "Data", \
					f(x) with lines lw 3 dashtype 2 linecolor rgb "#050DE1435" title sprintf("Fit: (x/%.f)^{%.2f} + %.3f", a, c, b) ;\
		' | gnuplot

# Main command-line output.
Out.txt : main.exe
	mono $< > $@

# Data
Out.times.txt : resources.exe
	>$@
	for N in $$(seq 10 5 300); do \
		mono $< -size:$$N >> $@;\
	done;

# Executables.
main.exe : main.cs LinearAlgebra.dll
	mcs -target:exe -out:$@ -reference:LinearAlgebra.dll $<
resources.exe : resources.cs LinearAlgebra.dll
	mcs -target:exe -out:$@ -reference:LinearAlgebra.dll $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)Matrix.cs $(LIBSPATH)Vector.cs $(LIBSPATH)QRGS.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean :
	$(RM) *.exe Out.* *.dll *.svg