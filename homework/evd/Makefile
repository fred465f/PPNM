all : Out.txt Out.rMaxConvergence.gnuplot.png Out.drConvergence.gnuplot.png Out.waveVectors.gnuplot.png Out.resources.gnuplot.png 

# Path to external self-made library.
LIBSPATH = ../../Libraries/

# Image files.
Out.rMaxConvergence.gnuplot.png : Out.rMaxConvergenceData.txt
	echo '\
			set terminal png background "white" ;\
			set out "$@" ;\
			set xlabel "r_{max}" ;\
			set ylabel "GS-energy (Hartree)" ;\
			set grid ;\
			set title "Convergence of GS-energy for varying r_{max}" ;\
			plot "$<" with lines lw 3 linecolor rgb "#FC695E" notitle ;\
		' | gnuplot
Out.drConvergence.gnuplot.png : Out.drConvergenceData.txt
	echo '\
			set terminal png background "white" ;\
			set out "$@" ;\
			set xlabel "dr" ;\
			set ylabel "GS-energy (Hartree)" ;\
			set grid ;\
			set title "Convergence of GS-energy for varying dr" ;\
			plot "$<" with lines lw 3 linecolor rgb "#FC695E" notitle ;\
		' | gnuplot	
Out.waveVectors.gnuplot.png : Out.numericalWaveFunctionsData.txt Out.analyticalWaveFunctionsData.txt
	echo '\
			set terminal png background "white" size "700, 1000" ;\
			set out "$@" ;\
			set grid ;\
			set key top center box height 1 ;\
			;\
			set multiplot layout 2, 1 spacing 0.08, 0.08 ;\
			;\
			set size 1, 0.52 ;\
			set format x " " ;\
			set yrange [0: 1] ;\
			set ytics 0.1, 0.2, 0.9 ;\
			set ylabel "Prob. amp." ;\
			set title "S-wave radial wave-functions with quantum numbers k" ;\
			plot "Out.numericalWaveFunctionsData.txt" using 1:2 with lines lw 3 linecolor rgb "#F05D5A" title "Numerical (k = 1)", \
					"Out.analyticalWaveFunctionsData.txt" using 1:2 with lines lw 3 lt 0 linecolor rgb "#3F9EEB" title "Analytical (k = 1)" ;\
			;\
			unset title ;\
			set size 1, 0.52 ;\
			unset format x ;\
			set yrange [-0.6: 0.4] ;\
			set ytics -0.5, 0.2, 0.3 ;\
			set xlabel "r (in units of Bohr radius)" ;\
			set ylabel "Prob. amp." ;\
			plot "Out.numericalWaveFunctionsData.txt" using 1:3 with lines lw 3 linecolor rgb "#F05D5A" title "Numerical (k = 2)", \
					"Out.analyticalWaveFunctionsData.txt" using 1:3 with lines lw 3 lt 0 linecolor rgb "#3F9EEB" title "Analytical (k = 2)" ;\
			;\
			unset multiplot ;\
		' | gnuplot
Out.resources.gnuplot.png : Out.resourcesData.txt
	echo '\
			set terminal png background "white" ;\
			set out "$@" ;\
			set xlabel "Size of square matrix" ;\
			set ylabel "Diagonalization. runtime (s)" ;\
			set grid ;\
			set title "Diagonalization runtime vs. square matrix size" ;\
			f(x) = b + (x/a)**c ;\
			b = 1; a = 90; c = 3 ;\
			fit f(x) "$<" via a, b ;\
			plot "$<" with lines lw 3 linecolor rgb "#F05D5A" title "Data", \
					f(x) with lines lw 3 linecolor rgb "#3F9EEB" title sprintf("Fit: (x/%.f)^{%.2f} + %.3f", a, c, b) ;\
		' | gnuplot

# Data.
Out.rMaxConvergenceData.txt : convergence.exe
	> $@
	for rMax in $$(seq 3 0.35 10); do \
		mono $< -rMax:$$rMax -dr:0.3 -test:rMax >> $@; \
	done
Out.drConvergenceData.txt : convergence.exe
	> $@
	for dr in $$(seq 0.05 0.05 0.5); do \
		mono $< -rMax:14 -dr:$$dr -test:dr >> $@; \
	done
Out.resourcesData.txt : resources.exe
	>$@
	for N in $$(seq 100 5 300); do \
		time --format "$$N %e" --output $@ --append \
		mono $< -size:$$N;\
	done
Out.numericalWaveFunctionsData.txt : numericalWaveFunctions.exe
	mono $< -rMax:14 -dr:0.3 -outFile:$@
Out.analyticalWaveFunctionsData.txt : analyticalWaveFunctions.exe
	mono $< -rMax:14 -dr:0.3 -outFile:$@

# Main command-line output.
Out.txt : main.exe
	mono $< -rMax:14 -dr:0.05 -outFile:Out.numericalWaveVectorsData.txt,Out.analyticalWaveVectorsData.txt > $@

# Executables.
main.exe : main.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
convergence.exe : convergence.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
resources.exe : resources.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
numericalWaveFunctions.exe : numericalWaveFunctions.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<
analyticalWaveFunctions.exe : analyticalWaveFunctions.cs LinearAlgebra.dll
	mcs -target:exe -reference:LinearAlgebra.dll -out:$@ $<

# Libraries.
LinearAlgebra.dll : $(LIBSPATH)LinearAlgebra.cs
	mcs -target:library -out:$@ $<

# Rule to clean.
clean :
	$(RM) *.exe [Oo]ut.* *.dll *.log
